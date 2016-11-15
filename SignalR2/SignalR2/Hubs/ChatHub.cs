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

        public static Dictionary<string, string> usersIds = new Dictionary<string, string>();
        public static int userCounter;

        public void SendToAll(string msg)
        {
            Clients.Others.Hello(msg);
        }

        public void sendInfo(int userCounter)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.online(userCounter);
        }

        public override Task OnConnected()
        {

            var name = Context.User.Identity.Name;
            var id = Context.ConnectionId;

            usersIds.Add(name, id);
            userCounter++;

            sendInfo(userCounter);

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            var name = Context.User.Identity.Name;
            var id = Context.ConnectionId;

            usersIds.Add(name, id);
            userCounter++;

            sendInfo(userCounter);

            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var name = Context.User.Identity.Name;

            usersIds.Remove(name);
            userCounter--;

            sendInfo(userCounter);

            return base.OnDisconnected(stopCalled);
        }
    }

    public interface IClientHandler
    {
        void Hello(string msg);
    }
}