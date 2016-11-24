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
    public class ChatHub : Hub<IClientHandler>
    {
        [Authorize()]
        public void SendToAll(string msg)
        {
            Clients.Others.Hello(msg);
        }

        public override Task OnConnected()
        {
            this.Clients.Caller.Hello("Hello from serverek!");
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }

    public interface IClientHandler
    {
        void Hello(string msg);
    }
}