using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Infrastructure;
using Store.Models.Repositories.Abstract;
using Store.Models;
using Store.Models.ViewModels;

namespace Store.Controllers
{
    public class CartController : Controller 
    {
        private readonly IProductRepository _repository;
        private Cart cart;
        public CartController(IProductRepository repository, Cart carService)
        {
            _repository = repository;
            cart = carService;
        }

        public ViewResult Index (string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToActionResult AddToCart(Guid productId, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {                
                cart.AddItem(product, 1);               
            }
            return RedirectToAction("Index", new {returnUrl});
        }

        public RedirectToActionResult RemoveFromCart(Guid productId, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {                
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        [HttpPost]
        public RedirectToActionResult UpQuantity(Guid productId, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if(product != null)
            {
                cart.UpQuantity(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult DownQuantity(Guid productId, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                cart.DownQuantity(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
    }
}
