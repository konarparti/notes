using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

            var result = controller.ShowListProducts(null, 2).ViewData.Model as ProductsListViewModel;

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

            var result = controller.ShowListProducts(null, 2).ViewData.Model as ProductsListViewModel; //действие

            //утверждение
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }


        //модульный тест для проверки функциональности фильтрации по категории
        //проверяется корректность генерации сведений о товарах указанной категории
        [Fact]
        public void CanFilterProducts()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
               new Product
                {
                    ProductID = new Guid("054907fb-4ac2-4772-ac91-b5bf4f1fca60"),
                    Name = "P1",
                    Category = "C1"              
                },
                new Product
                {
                    ProductID = new Guid("17bc93a9-ae2c-461c-9147-8e017e10aa7f"),
                    Name = "P2",
                    Category = "C2"
                },
                new Product
                {
                    ProductID = new Guid("2245a2f8-c292-42d8-8914-ae74ee782a72"),
                    Name = "P3",
                    Category = "C1"
                },
                new Product
                {
                    ProductID = new Guid("31fd93fe-84eb-41d5-b386-bac8a90fbeb9"),
                    Name = "P4",
                    Category = "C2"
                },
                new Product
                {
                    ProductID = new Guid("41fd93fe-84eb-41d5-b386-bac8a90fbeb9"),
                    Name = "P5",
                    Category = "C3"
                }
            }).AsQueryable());
            var controller = new ProductController(mock.Object) { PageSize = 3};

            Product[] result = (controller.ShowListProducts("C2", 1).ViewData.Model as ProductsListViewModel).Products.ToArray();

            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "C2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "C2");

        }

        //модульный тест возможности генерации корректных счетчиков товаров для различных категорий
        [Fact]
        public void GenerateCategorySpecificProductCount()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
               new Product
                {
                    ProductID = new Guid("054907fb-4ac2-4772-ac91-b5bf4f1fca60"),
                    Name = "P1",
                    Category = "C1"
                },
                new Product
                {
                    ProductID = new Guid("17bc93a9-ae2c-461c-9147-8e017e10aa7f"),
                    Name = "P2",
                    Category = "C2"
                },
                new Product
                {
                    ProductID = new Guid("2245a2f8-c292-42d8-8914-ae74ee782a72"),
                    Name = "P3",
                    Category = "C1"
                },
                new Product
                {
                    ProductID = new Guid("31fd93fe-84eb-41d5-b386-bac8a90fbeb9"),
                    Name = "P4",
                    Category = "C2"
                },
                new Product
                {
                    ProductID = new Guid("41fd93fe-84eb-41d5-b386-bac8a90fbeb9"),
                    Name = "P5",
                    Category = "C3"
                }
            }).AsQueryable());
            var target = new ProductController(mock.Object) { PageSize = 3 };
            Func<ViewResult, ProductsListViewModel> GetModel = result => result?.ViewData?.Model as ProductsListViewModel;

            //действие
            int? result1 = GetModel(target.ShowListProducts("C1"))?.PagingInfo.TotalItems;
            int? result2 = GetModel(target.ShowListProducts("C2"))?.PagingInfo.TotalItems;
            int? result3 = GetModel(target.ShowListProducts("C3"))?.PagingInfo.TotalItems;
            int? resultAll = GetModel(target.ShowListProducts(null))?.PagingInfo.TotalItems;

            //утверждение
            Assert.Equal(2, result1);
            Assert.Equal(2, result2);
            Assert.Equal(1, result3);
            Assert.Equal(5, resultAll);
        }
    }
}
