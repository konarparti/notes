using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Store.Controllers;
using Store.Models;
using Store.Models.Repositories.Abstract;
using Store.Models.ViewModels;
using Xunit;

namespace Store.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void CanPaginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product
                {
                    ProductID = new Guid("054907fb-4ac2-4772-ac91-b5bf4f1fca60"),
                    Name = "P1"
                },
                new Product
                {
                    ProductID = new Guid("17bc93a9-ae2c-461c-9147-8e017e10aa7f"),
                    Name = "P2"
                },
                new Product
                {
                    ProductID = new Guid("2245a2f8-c292-42d8-8914-ae74ee782a72"),
                    Name = "P3"
                },
                new Product
                {
                    ProductID = new Guid("31fd93fe-84eb-41d5-b386-bac8a90fbeb9"),
                    Name = "P4"
                }
            }).AsQueryable());
            var controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            var result = controller.ShowListProducts(2).ViewData.Model as ProductsListViewModel;

            Product[] prodArr = result.Products.ToArray();
            Assert.True(prodArr.Length == 1);
            Assert.Equal("P4", prodArr[0].Name);
        }

       //TODO: Добавить тесты для данных разбиения на страницы для viewModel
       //протестировать: что контроллер отправляет представлению корректную информацию о разбиении на страницы.
        
        [Fact]
        public void CanSendPaginationViewModel()
        {
            var mock = new Mock<IProductRepository>(); 
            mock.Setup(m => m.Products).Returns((new Product[]
            {
               new Product
                {
                    ProductID = new Guid("054907fb-4ac2-4772-ac91-b5bf4f1fca60"),
                    Name = "P1"
                },
                new Product
                {
                    ProductID = new Guid("17bc93a9-ae2c-461c-9147-8e017e10aa7f"),
                    Name = "P2"
                },
                new Product
                {
                    ProductID = new Guid("2245a2f8-c292-42d8-8914-ae74ee782a72"),
                    Name = "P3"
                },
                new Product
                {
                    ProductID = new Guid("31fd93fe-84eb-41d5-b386-bac8a90fbeb9"),
                    Name = "P4"
                },
                new Product
                {
                    ProductID = new Guid("41fd93fe-84eb-41d5-b386-bac8a90fbeb9"),
                    Name = "P5"
                }
            }).AsQueryable());
            var controller = new ProductController(mock.Object) { PageSize = 3};

            var result = controller.ShowListProducts(2).ViewData.Model as ProductsListViewModel; //действие

            //утверждение
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }
    }
}
