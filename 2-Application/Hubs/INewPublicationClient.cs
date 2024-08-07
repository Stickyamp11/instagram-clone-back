using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram_Api.Application.Hubs
{
    public interface INewPublicationClient
    {
        Task ReceiveMessage(string message);
    }
}
