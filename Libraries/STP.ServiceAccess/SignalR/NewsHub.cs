using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using Unity;

namespace STP.ServiceAccess.SignalR
{
    [HubName("newsHub")]
    public class NewsHub : Hub
    {

        public void AddMessage(long contentId, string newsItem)
        {
            Clients.All.addMessage(contentId, newsItem);
        }
    }
}
