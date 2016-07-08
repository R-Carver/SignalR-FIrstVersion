using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace WebApplication1.Utilities.SignalR
{
    public class Messenger
    {
        private readonly static Lazy<Messenger> _instance = new Lazy<Messenger>(() => new Messenger(GlobalHost.ConnectionManager.GetHubContext<MessengerHub>().Clients));

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(700);

        //The information that is needed for displaying in the messenger
        private readonly ConcurrentDictionary<DateTime, string> _contractInfo = new ConcurrentDictionary<DateTime, string>();

        private Messenger(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;

        }

        public static Messenger Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext<dynamic> Clients { get; set; }

        public void AddToMessenger(DateTime dateCreated, string contractName)
        {
            _contractInfo.TryAdd(dateCreated, contractName);
            UpdateMessenger();
        }

        public void UpdateMessenger()
        {
            Clients.All.refreshBox();
            foreach (var contract in _contractInfo)
            {   
                //contract.Key is the Datetime, Value is the name
                this.BroadCastNewContract(contract.Key.ToString(), contract.Value);
            }
        }


        public void BroadCastNewContract(string dateCreated, string contractName)
        {
            Clients.All.showNewContractMessage(dateCreated, contractName);   
        }

    }
}