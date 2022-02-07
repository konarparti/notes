using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Store.Controllers;
using Store.Models;
using Store.Models.Repositories.Abstract;
using Xunit;

namespace Store.Tests
{
    public class AdminControllerTests
    {
        /// <summary>
        /// тестирование метода действия Index(), проверка корректного возврата объектов Product из хранилища
        /// </summary>
        [Fact]
        public void IndexContainsAllProducts()
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
                }
            }).AsQueryable());
            var target = new AdminController(mock.Object);

            Product[] result = GetViewModel<IEnumerable<Product>>(target.Index())?.ToArray();

            Assert.Equal(3, result.Length);
            Assert.Equal("P1", result[0].Name);
            Assert.Equal("P2", result[1].Name);
            Assert.Equal("P3", result[2].Name);
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }


        /// <summary>
        /// тестирование метода действия Edit. Проверка корректности получения запрашиваемого товара, предоставляю методу корректное значение ID
        /// </summary>
        [Fact]
        public void CanEditProduct()
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
                }
            }).AsQueryable());
            var target = new AdminController(mock.Object);

            Product p1 = GetViewModel<Product>(target.Edit(new Guid("054907fb-4ac2-4772-ac91-b5bf4f1fca60")));
            Product p2 = GetViewModel<Product>(target.Edit(new Guid("17bc93a9-ae2c-461c-9147-8e017e10aa7f")));
            Product p3 = GetViewModel<Product>(target.Edit(new Guid("2245a2f8-c292-42d8-8914-ae74ee782a72")));

            Assert.Equal(new Guid("054907fb-4ac2-4772-ac91-b5bf4f1fca60"), p1.ProductID);
            Assert.Equal(new Guid("17bc93a9-ae2c-461c-9147-8e017e10aa7f"), p2.ProductID);
            Assert.Equal(new Guid("2245a2f8-c292-42d8-8914-ae74ee782a72"), p3.ProductID);
        }

        /// <summary>
        /// тестирование метода действия Edit. Проверка работы при отсутствии запрашиваемого товара в хранилище
        /// </summary>
        [Fact]
        public void CannotEditNonexistentProduct()
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
                }
            }).AsQueryable());
            var target = new AdminController(mock.Object);

            Product result = GetViewModel<Product>(target.Edit(new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")));

            Assert.Null(result);
        }

        /// <summary>
        /// тестирование метода действия Edit [HTTPPOST]. Проверка того, что хранилищу передаются допустимые обновления объекта Product, полученные как аргумент
        /// </summary>
        [Fact]
        public void CanSaveValidChanges()
        {
            var mock = new Mock<IProductRepository>();
            var tempData = new Mock<ITempDataDictionary>();
            var target = new AdminController(mock.Object) { TempData = tempData.Object};
            Product product = new Product { Name = "Test" };//создание товара

            IActionResult result = target.Edit(product);//попытка сохранить товар

            mock.Verify(m => m.SaveProduct(product));//проверка того, что к хранилищу было произведено обращение
            Assert.IsType<RedirectToActionResult>(result);//проверка того, что тип результата является перенаправление
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);//проверка того, что перенаправление произошло на Index
        }


        /// <summary>
        /// тестирование метода действия Edit [HTTPPOST]. Проверка того, что недопустимые обновления (с ошибкой проверки достоверности) не передаются в хранилище
        /// </summary>
        [Fact]
        public void CannotSaveInvalidChanges()
        {
            var mock = new Mock<IProductRepository>();
            var target = new AdminController(mock.Object);
            var product = new Product { Name = "Test" };
            target.ModelState.AddModelError("error", "error");//добавление ошибки в состояние модели

            IActionResult result = target.Edit(product); //попытка сохранить

            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());// проверка того, что к хранилищу был запрос
            Assert.IsType<ViewResult>(result); // проверка типа результата
        }

        /// <summary>
        /// тестирование метода действия Delete [HTTPPOST]. Проверка корректного удаления товара при корректных значениях параметров
        /// </summary>
        [Fact]
        public void CanDeleteValidProducts()
        {
            var prod = new Product { ProductID = new Guid("2245a2f8-c292-42d8-8914-ae74ee782a72"), Name = "Test" };
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
           {
                new Product
                {
                    ProductID = new Guid("054907fb-4ac2-4772-ac91-b5bf4f1fca60"),
                    Name = "P1"
                },
                prod,
                new Product
                {
                    ProductID = new Guid("2245a2f8-c292-42d8-8914-ae74ee782a72"),
                    Name = "P3"
                }
           }).AsQueryable());
            var target = new AdminController(mock.Object);

            target.Delete(prod.ProductID);

            mock.Verify(m => m.DeleteProduct(prod.ProductID));//проверка того, что был вызван метод удаления в хранилище с корректным объектом Product
        }
    }
}
