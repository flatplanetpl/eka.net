using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalR2.Hubs
{
    [HubName("chat")]
    [Authorize()]
    public class ChatHub : Hub<IClientHandler>
    {

        public void SendToAll(string msg)
        {

            Clients.All.Hello(HttpContext.Current.User.Identity.Name + " napisał: \n" + msg);
        }
    }

    public interface IClientHandler
    {
        void Hello(string msg);
    }
}