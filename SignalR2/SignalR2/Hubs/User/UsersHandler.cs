using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignalR2.Hubs
{
    public class UsersHandler
    {
        public Dictionary<string, string> ConnectedClients;
        public UsersHandler()
        {
            ConnectedClients = new Dictionary<string, string>();
        }

        public int Size()
        {
            return ConnectedClients.Count;
        }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(ConnectedClients);
        }
    }
    public enum ActionType
    {
        Connecting,
        Disconnecting,
        Drawing,
        Clearing,
    }
}