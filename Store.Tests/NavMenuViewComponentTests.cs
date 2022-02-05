using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Store.Models.Repositories.Abstract;
using Store.Models;
using Store.Controllers;
using Store.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Store.Tests
{
    public class NavMenuViewComponentTests
    {

        //модульный тест для проверки возможности генерации списка категорий
        //проверяется что создается список категорий в алфавитном порядке без дубликатов
        [Fact]
        public void CanSelectCategories()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
               new Product
                {
                    ProductID = new Guid("054907fb-4ac2-4772-ac91-b5bf4f1fca60"),
                    Name = "P1",
                    Category = "A"
                },
                new Product
                {
                    ProductID = new Guid("17bc93a9-ae2c-461c-9147-8e017e10aa7f"),
                    Name = "P2",
                    Category = "A"
                },
                new Product
                {
                    ProductID = new Guid("2245a2f8-c292-42d8-8914-ae74ee782a72"),
                    Name = "P3",
                    Category = "B"
                },
                new Product
                {
                    ProductID = new Guid("31fd93fe-84eb-41d5-b386-bac8a90fbeb9"),
                    Name = "P4",
                    Category = "C"
                },

            }).AsQueryable());
            var target = new NavMenuViewComponent(mock.Object);

            var result = ((IEnumerable<string>)(target.Invoke() as ViewViewComponentResult).ViewData.Model).ToList();

            Assert.True(Enumerable.SequenceEqual(new string[] { "A", "B", "C" }, result));
        }

        //модульный тест для проверки того, что компонент представления корректно добавляет детали о выбранной категории
        //в тесте читаем значение свойства ViewBag
        [Fact]
        public void IndicatesSelectedCategory()
        {
            string category = "A";
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
               new Product
                {
                    ProductID = new Guid("054907fb-4ac2-4772-ac91-b5bf4f1fca60"),
                    Name = "P1",
                    Category = "A"
                },
                new Product
                {
                    ProductID = new Guid("17bc93a9-ae2c-461c-9147-8e017e10aa7f"),
                    Name = "P2",
                    Category = "B"
                }
            }).AsQueryable());
            var target = new NavMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new Microsoft.AspNetCore.Routing.RouteData()
                }
            };
            target.RouteData.Values["category"] = category;

            var result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];

            Assert.Equal(category, result);
        }
    }
}