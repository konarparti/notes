
//Тестирование класса Cart 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Store.Models;

namespace Store.Tests
{
    public class CartTests
    {
        ///тестирование добавления элемента в корзину
        [Fact]
        public void CanAddNewLines()
        {
            var prod1 = new Product
            {
                ProductID = new Guid("054907fb-4ac2-4772-ac91-b5bf4f1fca60"),
                Name = "P1",
                Category = "C1"
            };
            var prod2 = new Product
            {
                ProductID = new Guid("17bc93a9-ae2c-461c-9147-8e017e10aa7f"),
                Name = "P2",
                Category = "C2"
            };
            Cart target = new Cart();

            target.AddItem(prod1, 1);
            target.AddItem(prod2, 2);
            CartLine[] results = target.Lines.ToArray();

            Assert.Equal(2, results.Length);
            Assert.Equal(prod1, results[0].Product);
            Assert.Equal(prod2, results[1].Product);
        }

        ///тестирование добавления в корзину уже имеющегося в ней элемента => нкжно увеличить кол-вл этого элемента в корзине
        [Fact]
        public void CanAddQuantityForExistingLines()
        {
            var prod1 = new Product
            {
                ProductID = new Guid("054907fb-4ac2-4772-ac91-b5bf4f1fca60"),
                Name = "P1",
                Category = "C1"
            };
            var prod2 = new Product
            {
                ProductID = new Guid("17bc93a9-ae2c-461c-9147-8e017e10aa7f"),
                Name = "P2",
                Category = "C2"
            };
            Cart target = new Cart();

            target.AddItem(prod1, 1);
            target.AddItem(prod2, 2);
            target.AddItem(prod1, 10);
            CartLine[] results = target.Lines.OrderBy(c => c.Product.ProductID).ToArray();

            Assert.Equal(2, results.Length);
            Assert.Equal(11, results[0].Quantity);
            Assert.Equal(2, results[1].Quantity);
        }

        ///тестирование возможности удаления товара из корзины
        [Fact]
        public void CanRemoveList()
        {
            var prod1 = new Product
            {
                ProductID = new Guid("054907fb-4ac2-4772-ac91-b5bf4f1fca60"),
                Name = "P1",
                Category = "C1"
            };
            var prod2 = new Product
            {
                ProductID = new Guid("17bc93a9-ae2c-461c-9147-8e017e10aa7f"),
                Name = "P2",
                Category = "C2"
            };
            var prod3 = new Product
            {
                ProductID = new Guid("27bc93a9-ae2c-461c-9147-8e017e10aa7f"),
                Name = "P3",
                Category = "C2"
            };
            Cart target = new Cart();
            target.AddItem(prod1, 1);
            target.AddItem(prod2, 3);
            target.AddItem(prod3, 5);
            target.AddItem(prod1, 1);

            target.RemoveLine(prod2);

            Assert.Equal(0, target.Lines.Where(t => t.Product == prod2).Count());
            Assert.Equal(2, target.Lines.Count());
        }

        ///тестирования вычисления общей стоимости
        [Fact]
        public void CalculateCartTotal()
        {
            var prod1 = new Product
            {
                ProductID = new Guid("054907fb-4ac2-4772-ac91-b5bf4f1fca60"),
                Name = "P1",
                Price = 200m
            };
            var prod2 = new Product
            {
                ProductID = new Guid("17bc93a9-ae2c-461c-9147-8e017e10aa7f"),
                Name = "P2",
                Price = 144m
            };
            Cart target = new Cart();
            target.AddItem(prod1, 1);
            target.AddItem(prod2, 1);
            target.AddItem(prod1, 3);
            decimal result = target.ComputeTotalValue();

            Assert.Equal(944m, result);
        }

        ///тестирование полной очистки корзины
        [Fact]
        public void CanClearCart()
        {
            var prod1 = new Product
            {
                ProductID = new Guid("054907fb-4ac2-4772-ac91-b5bf4f1fca60"),
                Name = "P1",
                Price = 200m
            };
            var prod2 = new Product
            {
                ProductID = new Guid("17bc93a9-ae2c-461c-9147-8e017e10aa7f"),
                Name = "P2",
                Price = 144m
            };
            Cart target = new Cart();
            target.AddItem(prod1, 1);
            target.AddItem(prod2, 1);

            target.ClearCart();

            Assert.Equal(0, target.Lines.Count());
        }
    }
}
