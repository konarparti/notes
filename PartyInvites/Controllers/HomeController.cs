using Microsoft.AspNetCore.Mvc;
using PartyInvites.Models;
using System.Diagnostics;

namespace PartyInvites.Controllers
{
    public class HomeController : Controller
    {
        public IRepository repository = Repository.SharedRepository;
        public IActionResult Index()
        {
            return View("HomePage");
        }

        [HttpGet]
        public ViewResult AnswerForm()
        {
            return View();
        }

        [HttpPost]
        public ViewResult AnswerForm(GuestResponse guestResponse)
        {
            if (ModelState.IsValid)
            {
                repository.AddResponse(guestResponse);
                return View("Thanks", guestResponse);
            }
            return View(); //обнаружена ошибка проверки достоверности
        }

        public ViewResult ListResponses()
        {
            return View(repository.Responses.Where(a => a.WillAttend == true));
        }
    }
}