
//класс реализует интерфейс IProductRepository возвращая в качестве значения свойства Products фиксированную коллекцию объектов Product
//Метод AsQueryable() применяется для преобразования фикисрованной коллекции объектов в IQueryable<Product>

using Store.Models.Repositories.Abstract;

namespace Store.Models.Repositories.Fake
{
    public class FakeProductRepository : IProductRepository
    {
        public IQueryable<Product> Products => new List<Product>()
        {
            new Product {Name = "Football", Price = 30},
            new Product {Name ="Basketball", Price = 25},
            new Product {Name = "Surf Board", Price = 345}
        }.AsQueryable();
    }
}
