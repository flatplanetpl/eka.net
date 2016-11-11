using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalR2.Extensions;

namespace SignalR2.Hubs
{
    [HubName("puns")]
    public class PunsHub : Hub<IPunsClientHandler>
    {
        public static List<string> Images = new List<string>();
        public static UsersHandler Users = new UsersHandler();

        public override Task OnConnected()
        {
            AddClient(Context.ConnectionId,GetIpAddress());
            Clients.Caller.LoadImage(Images);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            RemoveClient(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public void SendPath(string path)
        {
            Images.Add(path);
            Clients.Others.DrawPath(path);
            Clients.All.Action(DateTime.Now.TimeOfDay, ActionType.Drawing.ToString(), Context.ConnectionId);
        }

        public void Clear()
        {
            Images.Clear();
            Clients.All.Clear();
            Clients.All.Action(DateTime.Now.TimeOfDay, ActionType.Clearing.ToString(), Context.ConnectionId);
        }

        private void AddClient(string id, string ip)
        {
            Users.ConnectedClients.Add(id,ip);
            Clients.All.NumberOfUsers(Users.Size());
            Clients.All.Action(DateTime.Now.TimeOfDay,ActionType.Connecting.ToString(), id);
            Clients.All.Users(Users.GetJson());
        }
        private void RemoveClient(string id)
        {
            Users.ConnectedClients.Remove(id);
            Clients.All.NumberOfUsers(Users.Size());
            Clients.All.Action(DateTime.Now.TimeOfDay, ActionType.Disconnecting.ToString(), id);
            Clients.All.Users(Users.GetJson());
        }
        private string GetIpAddress()
        {
            return Context.Request.GetRemoteIpAddress();
        }
    }

    public interface IPunsClientHandler
    {
        void DrawPath(string path);
        void Clear();
        void LoadImage(List<string> image);
        void NumberOfUsers(int volume);
        void Users(string json);
        void Action(TimeSpan data, string action, string user);
    }

}