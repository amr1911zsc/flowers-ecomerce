using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication6.Data;
using WebApplication6.Models;
using WebApplication6.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace WebApplication6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddtocartController : ControllerBase
    {
        Appcontext appcontext;
        public AddtocartController( Appcontext appcontext)
        {
            this.appcontext = appcontext;
        }
        [HttpPost("AddProductToCart")]
        [Authorize]
        public IActionResult AddProductToCart(AddToCartDto products)
        {
            if (ModelState.IsValid)
            {
                
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var UserCart = appcontext.Carts.FirstOrDefault(c => c.UserID == userId);
                if (UserCart == null)
                {
                    //add cart for this user
                    Cart cart = new Cart();
                    cart.UserID = userId;
                    appcontext.Carts.Add(cart);
                    appcontext.SaveChanges();
                }
                else
                {
                    //check if user have product that want to add again
                    CartItem productItem = appcontext.CartItems.FirstOrDefault(ci => ci.CartID== UserCart.CartID && ci.ProductID == products.ProductId);
                    //if exist
                    if (productItem != null)
                    {
                        productItem.Quantity += products.Quantity;
                        appcontext.SaveChanges();
                    }
                    else
                    {
                        CartItem cartItem = new CartItem();
                        cartItem.CartID = UserCart.CartID;
                        cartItem.Quantity = products.Quantity;
                        cartItem.ProductID = products.ProductId;
                        appcontext.CartItems.Add(cartItem);
                        appcontext.SaveChanges();
                    }

                }
                return Ok(new { message = "the product has successfully added to cart" });
            }
            return BadRequest(ModelState);
        }
        [HttpPut("EditProductInCart/{productId:int}")]
        [Authorize]
        public IActionResult EditProductInCart(int productId, AddToCartDto prouctFromUser)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userCart = appcontext.Carts.FirstOrDefault(c => c.UserID == userId);
                CartItem product = appcontext.CartItems.FirstOrDefault(c => c.ProductID == productId);
                if (product != null)
                {
                    product.ProductID = prouctFromUser.ProductId;
                    product.Quantity = prouctFromUser.Quantity;
                    appcontext.CartItems.Update(product);
                    appcontext.SaveChanges();
                    return Ok(new { message = $"product updated successfully" });
                }
                return NotFound(new { message = "product not found" });
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteProductFromCart/{productId:int}")]
        [Authorize]
        public IActionResult DeleteProductFromCart(int productId)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userCart = appcontext.Carts.FirstOrDefault(c => c.UserID == userId);
                CartItem product = appcontext.CartItems.FirstOrDefault(c => c.ProductID == productId);
                if (product != null)
                {
                    appcontext.CartItems.Remove(product);
                    appcontext.SaveChanges();
                    return Ok(new { message = "product deleted successfully" });
                }
                return NotFound(new { message = "product not found" });
            }
            return BadRequest(ModelState);
        }
        [HttpGet("ShowAllProductsInCart")]
        [Authorize]
        public IActionResult ShowCartForUser()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Cart userCard = appcontext.Carts.FirstOrDefault(c => c.UserID == userId);
            if (userCard != null)
            {

                List<CartItem> cartItems = appcontext.CartItems.Include(ci => ci.Product).ToList();
                List<ShowProductsInCardDTO> show = new List<ShowProductsInCardDTO>();
                foreach (CartItem cartItem in cartItems)
                {
                    ShowProductsInCardDTO showItem = new ShowProductsInCardDTO();
                    showItem.ProductName = cartItem.Product.Name;
                    showItem.Quantity = cartItem.Quantity;
                    show.Add(showItem);
                }
                return Ok(show);
            }
            else
            {
                return Ok(new { message = "Cart has no products" });
            }

            // List<CartItem> products = context.product.Items;
            return Ok();
        }
    }




}

