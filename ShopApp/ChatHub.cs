using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ShopApp.Models;

namespace ShopApp
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        public void TestMessage(string message)
        {
            //using (var db = new DAL.ShopContext())
            //{
            //    Message x = new Message();
            //}
            Clients.All.TestMessage(message);
        }

        public override Task OnConnected()
        {
            string clientId = Context.ConnectionId;
            string data = clientId;

            Debug.WriteLine("ClientID: " + clientId);

            Clients.Caller.receiveMessage("ChatHub", data);

            return base.OnConnected();
        }

        [HubMethodName("connectHub")]
        public void GetConnect(string username, string userid, string connectionid)
        {
            string count = "";
            string msg = "";
            string list = "";
            try
            {
                //count = GetCount().ToString();
                //msg = updaterec(username, userid, connectionid);
                //list = GetUsers(username);
            }
            catch (Exception d)
            {
                msg = "DB Error " + d.Message;
            }
            var id = Context.ConnectionId;
            string[] Exceptional = new string[1];
            Exceptional[0] = id;
            Clients.Caller.receiveMessage("RU", msg, list);
            Clients.AllExcept(Exceptional).receiveMessage("NewConnection", username + " " + id, count);
        }
    }
}