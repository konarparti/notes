using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyInvites.Models
{
    public class Repository : IRepository
    {
        private static Repository sharedRepository = new Repository();
        private List<GuestResponse> guestResponses = new List<GuestResponse>();

        public static Repository SharedRepository => sharedRepository;
        public IEnumerable<GuestResponse> Responses => guestResponses;
        public void AddResponse(GuestResponse response) => guestResponses.Add(response);
    }
}
