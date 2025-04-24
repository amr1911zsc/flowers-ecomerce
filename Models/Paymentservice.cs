using WebApplication6.Data;
using WebApplication6.Models;
using WebApplication6.Dto;
using Stripe;
using Stripe.Checkout;
namespace WebApplication6.Models
{
    public class Paymentservice : Ipaymentservice
    {
        private readonly 
        Appcontext appcontext;
        public Paymentservice(Appcontext appcontext)
        {
            this.appcontext = appcontext;
        }
        public Task<Cart> CreateAsync(string cartid)
        {
            throw new NotImplementedException();
        }
    }
}
