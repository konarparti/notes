
// в данном классе внедрение зависимостей позволяет объекту ProductController получать доступ к хранилищу приложения
// через интерфейс IProductRepository без необходимости знать о том, какой класс реализации был сконфигугирован
//Это позволит безболезненно заменить имитацию хранилища на реальную БД

using Microsoft.AspNetCore.Mvc;
using Store.Models.Repositories.Abstract;
using Store.Models.ViewModels;

namespace Store.Controllers
{
    public class ProductController : Controller
    {
        public int PageSize = 1;
        private readonly IProductRepository _productRepository; // это и конструктор ниже - внедрение зависимостей

        public ProductController(IProductRepository repository)
        {
            _productRepository = repository;
        }
        public ViewResult ShowListProducts(int productPage = 1)
            => View(new ProductsListViewModel
            {
                Products = _productRepository.Products.Skip((productPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = _productRepository.Products.Count()
                }
            });    
    }
}

                
