using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// тестирование обработки заказа только если в корзине призутствуют элементы и предоставлены достоверные данные для доставки
/// </summary>

using Xunit;
using Moq;
using Store.Models.Repositories.Abstract;
using Store.Models;
using Store.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Store.Tests
{
    public class OrderControllerTests
    {
        /// <summary>
        /// тест проверяет отсутствие возможности перехода к оплате при пустой корзине
        /// </summary>
        [Fact]
        public void CannotCheckoutEmptyCart()
        {
            var mock = new Mock<IOrderRepository>(); // создание имитированного хранилища заказов
            var cart = new Cart(); // создание пустой корзины
            var order = new Order(); //создание заказа
            var target = new OrderController(mock.Object, cart);

            ViewResult result = target.Checkout(order) as ViewResult;

            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);//проверка что заказ не был сохранен
            Assert.True(string.IsNullOrEmpty(result.ViewName));//проверка что метод возвращает стандартное представление
            Assert.False(result.ViewData.ModelState.IsValid);//проверка что представлению передана неопустимая модель
        }

        /// <summary>
        /// тест внедряет в модель представления ошибку, эмулирующую проблему, о которой сообщает средство привязки модели (введены некорректные данные о доставке)
        /// </summary>
        [Fact]
        public void CannotInvalidShippingDetails()
        {
            var mock = new Mock<IOrderRepository>(); // создание имитированного хранилища заказов
            var cart = new Cart(); // создание корзины
            cart.AddItem(new Product(), 1); //добавление 1 элемента
            var target = new OrderController(mock.Object, cart);
            target.ModelState.AddModelError("error", "error"); //добавление ошибки в модель

            ViewResult result = target.Checkout(new Order()) as ViewResult; // попытка перехода к оплате

            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);//проверка что заказ не был сохранен
            Assert.True(string.IsNullOrEmpty(result.ViewName));//проверка что метод возвращает стандартное представление
            Assert.False(result.ViewData.ModelState.IsValid);//проверка что представлению передана неопустимая модель
        }

        /// <summary>
        /// проверка что корректные заказы сохраняются должным образом
        /// </summary>
        [Fact]
        public void CanCheckoutAndSubmitOrder()
        {
            var mock = new Mock<IOrderRepository>(); // создание имитированного хранилища заказов
            var cart = new Cart(); 
            cart.AddItem(new Product(), 1);
            var target = new OrderController(mock.Object, cart);

            RedirectToActionResult result = target.Checkout(new Order()) as RedirectToActionResult;
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);//проверка что заказ был сохранен
            Assert.Equal("Completed", result.ActionName); //проверка что метод перенаправляется на действие Completed
            
        }
    }
}
