namespace Store.Models.Repositories.Abstract
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
    }
}
