namespace WebApplication.Features.Cart.Entities
{
    public interface IAddable
    {
        ActiveCart Add(Category.Product product);
    }
}