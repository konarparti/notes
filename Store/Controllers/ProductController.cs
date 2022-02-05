
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
        public ViewResult ShowListProducts(string? category, int productPage = 1)
            => View(new ProductsListViewModel
            {
                Products = _productRepository.Products.Where(p => p.Category == null || p.Category == category).Skip((productPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ? _productRepository.Products.Count() : _productRepository.Products.Where(a => a.Category == category).Count()
                },
                CurrentCategory = category
            });    
    }
}

                
