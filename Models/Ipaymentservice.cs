namespace WebApplication6.Models
{
    public interface Ipaymentservice
    {
        Task<Cart> CreateAsync(string cartid);

    }
}
