namespace WebApplication.Features.Cart.Entities
{
    public interface IAddable
    {
        ActiveCart Add(Product product);
    }
}