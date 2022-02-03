
//модульный тест проверяет что метод действия ListResponses передает представлению все объекты в хранилище
//также применена изоляция контроллера в модульном тесте 
//здесь не используется Moq технология

using Microsoft.AspNetCore.Mvc;
using PartyInvites.Controllers;
using PartyInvites.Models;
using System.Collections.Generic;
using Xunit;
using Moq;

namespace PartyInvites.Tests
{
    public class HomeControllerTests
    {      
        [Fact]
        public void ListResponsesActionModelIsComplete()
        {
            var mock = new Mock<IRepository>();
            mock.SetupGet(m => m.Responses).
                Returns(new List<GuestResponse> { new GuestResponse { Name = "Anton", Email = "exampleNew@email.com", PhoneNumber = "312", WillAttend = true } });

            var controller = new HomeController() { repository = mock.Object};

            var model = controller.ListResponses()?.ViewData.Model as IEnumerable<GuestResponse>;

            Assert.Equal(controller.repository.Responses, model, Comparer.Get<GuestResponse>((gr1, gr2) => gr1.Name == gr2.Name && gr1.Email == gr2.Email));
        }


        //тестирование дублирования запросов к хранилищу

        [Fact]
        public void RepositoryPropCalledOnce()
        {
            var mock = new Mock<IRepository>();
            mock.SetupGet(m => m.Responses)
                .Returns(new List<GuestResponse> { new GuestResponse { Name = "Anton", Email = "exampleNew@email.com", PhoneNumber = "312", WillAttend = true } });
            var controller = new HomeController() { repository = mock.Object };

            var result = controller.ListResponses();

            mock.Verify(m => m.Responses, Times.Once);
;
        }
    }
}
