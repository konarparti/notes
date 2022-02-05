using Microsoft.AspNetCore.Mvc;
using Store.Models.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Components
{
    public class NavMenuViewComponent : ViewComponent
    {
        private readonly IProductRepository _repository;
        public NavMenuViewComponent(IProductRepository repository)
        {
            _repository = repository;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(_repository.Products.Select(x => x.Category).Distinct().OrderBy(x => x));
        }
    }
}
