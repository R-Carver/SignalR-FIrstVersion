using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebApplication1.Utilities.SignalR
{   
    [HubName("MessengerHub")]
    public class MessengerHub : Hub
    {
        private readonly Messenger _messenger;

        public MessengerHub() : this(Messenger.Instance) { }

        public MessengerHub(Messenger messenger)
        {
            _messenger = messenger;
        }

            
        public void showNewContractMessage()
        {
            _messenger.UpdateMessenger();
        }

    }
}