using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using PartyInvites.Models;

namespace PartyInvites.Tests
{
    public class GuestTests
    {
        [Fact]
        public void CanChangeGuestName()
        {
            var gr = new GuestResponse { Name = "Ann", Email = "test@email.com", PhoneNumber = "123", WillAttend = true }; //организация (arrange)
            gr.Name = "Anton";//действие (act)
            Assert.Equal("Anton", gr.Name);//утверждение (assert)
        }
    }
}
