
//модульный тест проверяет что метод действия ListResponses передает представлению все объекты в хранилище
//также применена изоляция контроллера в модульном тесте

using Microsoft.AspNetCore.Mvc;
using PartyInvites.Controllers;
using PartyInvites.Models;
using System.Collections.Generic;
using Xunit;

namespace PartyInvites.Tests
{
    public class HomeControllerTests
    {
        class ModelCompleteFakeRepository : IRepository
        {
            public IEnumerable<GuestResponse> Responses { get; } = new List<GuestResponse>()
            {
                new GuestResponse{Name = "Ann", Email = "example@email.com", PhoneNumber = "123", WillAttend = true },
                new GuestResponse{Name = "Anton", Email = "exampleNew@email.com", PhoneNumber = "312", WillAttend = true }
            };
            public void AddResponse(GuestResponse response)
            {
                //не требуется для тестирования
            }
        }

        [Fact]
        public void ListResponsesActionModelIsComplete()
        {

            var controller = new HomeController();
            controller.repository = new ModelCompleteFakeRepository();

            var model = controller.ListResponses()?.ViewData.Model as IEnumerable<GuestResponse>;

            Assert.Equal(controller.repository.Responses, model, Comparer.Get<GuestResponse>((gr1, gr2) => gr1.Name == gr2.Name && gr1.Email == gr2.Email));
        }


        //тестирование дублирования запросов к хранилищу
        class PropertyOnceFakeRepository : IRepository
        {
            public int PropertyCounter { get; set; } = 0;
            public IEnumerable<GuestResponse> Responses
            {
                get
                {
                    PropertyCounter++;
                    return new List<GuestResponse> { new GuestResponse { Name = "Anton", Email = "exampleNew@email.com", PhoneNumber = "312", WillAttend = true } };
                }
            }

            public void AddResponse(GuestResponse response)
            {
                //не требуется для тестирования
            }
        }

        [Fact]
        public void RepositoryPropCalledOnce()
        {
            var repos = new PropertyOnceFakeRepository();
            var controller = new HomeController() { repository = repos };

            var result = controller.ListResponses();

            Assert.Equal(1, repos.PropertyCounter);
;
        }
    }
}
