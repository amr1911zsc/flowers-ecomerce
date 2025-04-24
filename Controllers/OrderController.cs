using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication6.Dto;
using WebApplication6.Data;
using WebApplication6.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Stripe.Checkout;

namespace WebApplication6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,  // لضمان التعامل مع الحلقات المرجعية
                MaxDepth = 64  // تعيين الحد الأقصى للعمق لتجنب تجاوز الحد
            };
        }
        Appcontext appcontext;
        private readonly StripeSettings _stripeSettings;
        public OrderController( Appcontext appcontext, IOptions<StripeSettings> stripeSettings)
        {
            this.appcontext = appcontext;
            _stripeSettings = stripeSettings.Value;
        }
        [HttpPost("CheckOut")]
        [Authorize]
        public async Task<IActionResult> CheckOut()
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                // Card card=context.card.Include(c=>c.Items).
                //جبت الكارت بتاع اليوزر

                var user = appcontext.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    return BadRequest(new { message = "User not found." });
                }
                Cart userCard = appcontext.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefault(c => c.UserID == userId);
                //جبت البرودكت اللى جوا الكارت دى
                // CartItem cartItem = context.cartItems.FirstOrDefault(ci => ci.CardId == userCard.Id);
                //decimal totalPrice = 0;
                if (userCard != null)
                {
                    decimal totalPrice = userCard.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);// cartItem.Product.Price * cartItem.Quantity;

                    //هعمل بقا الاوردر
                    Order order = new Order();
                    order.UserID = userId;
                    order.DateOfOrder = DateTime.Now;
                    order.TotalPrice = totalPrice;
                    order.DeliveryLocation = user.Location;
                    order.OrderStatus = OrderStatus.Pending;


                    appcontext.Orders.Add(order);

                    appcontext.SaveChanges();
                    //هعمل الاورودر ديتيلز لكل بردكت

                    foreach (var cardItem in userCard.CartItems)
                    {
                        OrderDetail orderDetails = new OrderDetail();
                        orderDetails.ProductID = cardItem.ProductID;
                        orderDetails.Quantity = cardItem.Quantity;
                        orderDetails.OrderNumber = order.OrderID;
                        orderDetails.PriceAtPurchase = cardItem.Product.Price;
                        
                        appcontext.OrderDetails.Add(orderDetails);

                    }

                    await appcontext.SaveChangesAsync();
                    
                    var domain = "https://localhost:7275";  // عدل الرابط حسب موقعك

                    var options = new SessionCreateOptions
                    {
                        PaymentMethodTypes = new List<string> { "card" },
                        LineItems = userCard.CartItems.Select(item => new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmountDecimal = item.Product.Price * 100,  // Stripe يستخدم السنت
                                Currency = "usd",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = item.Product.Name
                                }
                            },
                            Quantity = item.Quantity
                        }).ToList(),
                        Mode = "payment",
                        SuccessUrl = $"{domain}/order/success?orderId={order.OrderID}",
                        CancelUrl = $"{domain}/order/cancel?orderId={order.OrderID}",
                        Metadata = new Dictionary<string, string>
                {
                    { "order_id", order.OrderID.ToString() }
                }
                    };

                    var service = new SessionService();
                    var session = await service.CreateAsync(options);

                    appcontext.CartItems.RemoveRange(userCard.CartItems);
                    await appcontext.SaveChangesAsync();
                    return Ok(new
                    {
                        message = "Checkout session created and sent to Stripe successfully ✅",
                        
                        stripeSessionId = session.Id,
                        totalPrice = totalPrice
                    });




                    //appcontext.SaveChanges();

                    // appcontext.CartItems.RemoveRange(userCard.CartItems);
                    // appcontext.SaveChanges();

                    return Ok(totalPrice);


                }
                else
                {
                    return BadRequest(new { message = "cart has no products" });
                }

                


                /*OrderDetails orderDetails = new OrderDetails();
                orderDetails.Quantity = cartItem.Quantity;
                orderDetails.ProductId = cartItem.ProductId;
                //orderDetails.PriceAtPurchase=productsFromUser.
                orderDetails.PriceAtPurchase = cartItem.Product.Price;
               // decimal itemPrice = orderDetails.PriceAtPurchase * orderDetails.Quantity;
              //  decimal totalPrice = 0;
              //  totalPrice += itemPrice;
                context.card.Remove(userCard);
                context.SaveChanges();
                return Ok(totalPrice);*/
            }
            return BadRequest(ModelState);
        }
        [HttpGet("GetOrders")]
        [Authorize]
        public IActionResult GetOrders()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var orders = appcontext.Orders
                                   .Include(o => o.OrderDetails)
                                   .ThenInclude(od => od.Product)
                                   .Where(o => o.UserID == userId && o.OrderDetails.Any()) // ✅ الشرط هنا: بس الطلبات اللي فيها OrderDetails
                                   .ToList();

            var ordersDto = orders.Select(o => new OrderDto
            {
                OrderID = o.OrderID,
                TotalPrice = o.TotalPrice,
                DeliveryLocation = o.DeliveryLocation,
                OrderStatus=o.OrderStatus.ToString(),
                OrderDetails = o.OrderDetails.Select(od => new OrderDetailDto
                {
                    ProductID = od.ProductID,
                    PriceAtPurchase = od.PriceAtPurchase,
                    Quantity = od.Quantity
                }).ToList()
            }).ToList();

            return Ok(ordersDto);
        }

        [HttpPut("UpdateOrderStatus/{orderId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateOrderStatus(int orderId, [FromBody] int newStatus)
        {
            var order = appcontext.Orders.FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)
            {
                return NotFound(new { message = "Order not found." });
            }

            // تأكد أن الحالة المرسلة قيمة صحيحة من الـ Enum
            if (!Enum.IsDefined(typeof(OrderStatus), newStatus))
            {
                return BadRequest(new { message = "Invalid status value." });
            }

            order.OrderStatus = (OrderStatus)newStatus;
            appcontext.SaveChanges();

            return Ok(new { message = $"Order status updated to {((OrderStatus)newStatus).ToString()}." });
        }
        [HttpDelete("CancelOrder/{orderId}")]
        [Authorize]
        public IActionResult CancelOrder(int orderId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // جلب الأوردر بناءً على الـ orderId و userId للتأكد إن العميل هو اللي طلب الإلغاء
            var order = appcontext.Orders
                                  .FirstOrDefault(o => o.OrderID == orderId && o.UserID == userId);

            if (order == null)
            {
                return BadRequest(new { message = "Order not found or you don't have permission to cancel this order." });
            }

            // التحقق إذا كانت حالة الأوردر هي "Pending" أو حالة أخرى تسمح بالإلغاء
            if (order.OrderStatus != OrderStatus.Pending)
            {
                return BadRequest(new { message = "Only pending orders can be cancelled." });
            }

            // تغيير حالة الأوردر إلى "Cancelled"
            order.OrderStatus = OrderStatus.Cancelled;

            appcontext.SaveChanges();

            return Ok(new { message = "Order successfully cancelled." });
        }
        [HttpGet("GetAllOrders")]
        [Authorize(Roles = "Admin")]  // تأكد أن المستخدم أدمن
        public IActionResult GetAllOrders()
        {
            // جلب جميع الأوردرات مع التفاصيل
            var orders = appcontext.Orders
                                   .Include(o => o.OrderDetails)
                                   .ThenInclude(od => od.Product)
                                   .ToList();

            if (orders == null || orders.Count == 0)
            {
                return NotFound(new { message = "No orders found." });
            }

            // تحويل البيانات إلى DTO (Data Transfer Object) لعرضها بشكل مرتب
            var ordersDto = orders.Select(o => new OrderDto
            {
                OrderID = o.OrderID,
                TotalPrice = o.TotalPrice,
                DeliveryLocation = o.DeliveryLocation,
                OrderStatus = o.OrderStatus.ToString(),  // لتحويل الحالة إلى نص
                DateOfOrder = o.DateOfOrder,
                OrderDetails = o.OrderDetails.Select(od => new OrderDetailDto
                {
                    ProductID = od.ProductID,
                    PriceAtPurchase = od.PriceAtPurchase,
                    Quantity = od.Quantity
                }).ToList()
            }).ToList();

            return Ok(ordersDto);
        }

    }
}
