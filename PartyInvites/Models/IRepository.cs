using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyInvites.Models
{
    public interface IRepository
    {
        IEnumerable<GuestResponse> Responses { get; }
        void AddResponse(GuestResponse response);
    }
}
