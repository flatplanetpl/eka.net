using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalR2.Hubs
{
    [HubName("puns")]
    public class PunsHub : Hub<IPunsClientHandler>
    {
        private static List<string> Image = new List<string>();
        private static string _lastPaintedPerson = "Jeszcze nikt nie malował";

        public override Task OnConnected()
        {
            Clients.Caller.LoadImage(Image);
            WhoLastPainted(_lastPaintedPerson);
            return base.OnConnected();
        }

        public void SendPath(string path)
        {
            Image.Add(path);
            Clients.Others.DrawPath(path);
            _lastPaintedPerson = HttpContext.Current.User.Identity.Name;
            WhoLastPainted(_lastPaintedPerson);

        }

        public void Clear()
        {
            Image.Clear();
            Clients.All.Clear();
        }
        public void WhoLastPainted(string identityName)
        {
            if(identityName != "") Clients.All.Who(identityName);
            else Clients.All.Who("Gość");
        }
    }

    public interface IPunsClientHandler
    {
        void DrawPath(string path);
        void Clear();
        void Who(string identityName);
        void LoadImage(List<string> image);

    }
}