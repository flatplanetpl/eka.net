using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalR2.Hubs
{
    public static class UserRepository
    {
        public static ConcurrentDictionary<string, IPrincipal> UserIds { get; } =
            new ConcurrentDictionary<string, IPrincipal>();

    }
    [HubName("chat")]
    public class ChatHub : Hub<IClientHandler>
    {

        public void SendToAll(string msg)
        {
            Clients.All.ShowMessage(msg);
        }

        public override Task OnConnected()
        {
            UserRepository.UserIds.TryAdd(Context.ConnectionId, Context.User);
            Clients.All.UpdateNumberOfClients(UserRepository.UserIds.Count);
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            UserRepository.UserIds.AddOrUpdate(Context.ConnectionId, Context.User, (key, oldUser) => Context.User);
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            IPrincipal iPrincipal;
            UserRepository.UserIds.TryRemove(Context.ConnectionId, out iPrincipal);
            Clients.All.UpdateNumberOfClients(UserRepository.UserIds.Count);
            return base.OnDisconnected(stopCalled);
        }
    }

    public interface IClientHandler
    {
        void UpdateNumberOfClients(int numberOfClients);
        void ShowMessage(string msg);
    }
}