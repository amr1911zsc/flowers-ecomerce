using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication6.Models;
using WebApplication6.Data;
using Stripe.Checkout;
using Stripe;



namespace WebApplication6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly Appcontext _context;
        private readonly IConfiguration _config;

        public WebhookController(Appcontext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            // مفتاح توقيع Stripe من إعدادات التطبيق
            var endpointSecret = _config["Stripe:WebhookSecret"];

            Event stripeEvent;

            try
            {
                var signatureHeader = Request.Headers["Stripe-Signature"];
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    signatureHeader,
                    endpointSecret
                );
            }
            catch (StripeException e)
            {
                return BadRequest($"⚠️ Webhook error: {e.Message}");
            }

            // لو الدفع تم بنجاح
            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;

                if (session != null && session.Metadata.TryGetValue("order_id", out var orderIdString))
                {
                    if (int.TryParse(orderIdString, out int orderId))
                    {
                        var order = await _context.Orders.FindAsync(orderId);
                        if (order != null)
                        {
                            order.OrderStatus = OrderStatus.Paid;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }

            return Ok();  // Stripe محتاج رد OK عشان مايعيدش الإرسال
        }
    }
}
