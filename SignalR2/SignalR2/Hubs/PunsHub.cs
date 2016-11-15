using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Concurrent;
using System.Security.Principal;

namespace SignalR2.Hubs
{
    [HubName("puns")]
    public class PunsHub : Hub<IPunsClientHandler>
    {
        static List<string> Image =new List<string>();
        private static ConcurrentDictionary<string, IPrincipal> Users = new ConcurrentDictionary<string, IPrincipal>();

        public override Task OnConnected()
        {
            Clients.Caller.LoadImage(Image);
            Users.TryAdd(Context.ConnectionId, Context.User);
            Clients.All.UpdateNumberOfClients(Users.Count);
            return base.OnConnected();
        }

        public void SendPath(string path)
        {
            Image.Add(path);
            Clients.Others.DrawPath(path);
        }

        public void Clear()
        {
            Image.Clear();
            Clients.All.Clear();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            IPrincipal principal;
            Users.TryRemove(Context.ConnectionId, out principal);
            Clients.All.UpdateNumberOfClients(Users.Count);
            return base.OnDisconnected(stopCalled);
        }
            
    }

    public interface IPunsClientHandler
    {
        void DrawPath(string path);
        void Clear();
        void LoadImage(List<string> image);
        void UpdateNumberOfClients(int count);
   
    }
}