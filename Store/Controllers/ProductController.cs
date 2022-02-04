
// в данном классе внедрение зависимостей позволяет объекту ProductController получать доступ к хранилищу приложения
// через интерфейс IProductRepository без необходимости знать о том, какой класс реализации был сконфигугирован
//Это позволит безболезненно заменить имитацию хранилища на реальную БД

using Microsoft.AspNetCore.Mvc;
using Store.Models.Repositories.Abstract;

namespace Store.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository; // строки 8 - 13 это внедрение зависимостей

        public ProductController(IProductRepository repository)
        {
            _productRepository = repository;        
        }
        public ViewResult ShowListProducts() => View(_productRepository.Products);
    }
}
