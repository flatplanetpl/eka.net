using Microsoft.AspNet.SignalR;

namespace SignalR2.Extensions
{
    public static class ExtensionHttpContext
    {
        public static string GetRemoteIpAddress(this IRequest request)
        {
            object ipAddress;
            if (request.Environment.TryGetValue("server.RemoteIpAddress", out ipAddress))
            {
                return ipAddress as string;
            }
            return "";
        }
    }
}