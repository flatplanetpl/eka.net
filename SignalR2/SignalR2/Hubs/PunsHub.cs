using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalR2.Hubs
{
    [HubName("puns")]
    public class PunsHub : Hub<IPunsClientHandler>
    {
        static List<string> Image =new List<string>();
        private static ConcurrentDictionary<string, IPrincipal> Users = new ConcurrentDictionary<string, IPrincipal>();

        public override Task OnConnected()
        {
            Users.TryAdd(Context.ConnectionId, Context.User);
            Clients.Caller.LoadImage(Image);
            Clients.All.UpdateNumberOfPeople(Users.Count); 
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            IPrincipal principal;
            Users.TryRemove(Context.ConnectionId, out principal);
            Clients.All.UpdateNumberOfPeople(Users.Count);
            return base.OnDisconnected(stopCalled);
        }

        public void SendPath(string path)
        {
            Image.Add(path);
            Clients.All.Log(Context.User.Identity.Name, DateTime.Now.ToString("G"));
            Clients.Others.DrawPath(path);
        }

        public void Clear()
        {
            Image.Clear();
            Clients.All.Clear();
        }
    }

    public interface IPunsClientHandler
    {
        void DrawPath(string path);
        void Clear();
        void LoadImage(List<string> image);
        void UpdateNumberOfPeople(int number);
        void Log(string name, string dateToString);
    }
}