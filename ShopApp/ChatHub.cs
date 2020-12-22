using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ShopApp.Models;

namespace ShopApp
{
    [Authorize]
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        private readonly static ConnectionMapping<string> connections = new ConnectionMapping<string>();

        public void SendMessage(string message, string userID)
        {
            Debug.WriteLine("Tutaj będzie dodawanie wiadomości do bazy?" + message);

            var id = Context.ConnectionId;
            Debug.WriteLine("ContextID?" + id);

            var userLogin = Context.User.Identity.Name;
            Debug.WriteLine("Wysyłam z: " + userLogin);
            Debug.WriteLine("Wysyłam do: " + userID);

            int.TryParse(userID, out int receiverID);

            using (var db = new DAL.ShopContext())
            {
                User sender = db.Users.Where(u => u.Login == userLogin).FirstOrDefault();
                User receiver = db.Users.Where(u => u.UserID == receiverID).FirstOrDefault();

                Message sendMessage = new Message()
                {
                    Sender = sender,
                    Receiver = receiver,
                    Content = message,
                    SentTime = DateTime.Now
                };

                db.Messages.Add(sendMessage);
                db.SaveChanges();

                receiver.ReceivedMessages.Add(sendMessage);
                db.SaveChanges();

                sender.SentMessages.Add(sendMessage);
                db.SaveChanges();
                Clients.User(sender.Login).sendMessage(message);
                Clients.User(receiver.Login).receiveMessage(message);
            }



            //if (HubHelper.IsConnected(targetUserName))
            //{
            //    Clients.User(targetUserName).sendMessage(message);
            //}
            //else
            //{
            //    DataAccess.InsertPendingMessage(userName, targetUserName, message);
            //}

            //Clients.Caller.receiveMessage(msgFrom, msg, touserid);
            //Clients.Client(touserid).receiveMessage(msgFrom, msg, id);

            //using (var db = new DAL.ShopContext())
            //{
            //    Message x = new Message();
            //}
            Clients.All.SendMessage(message, userID);
        }

        public void ReceiveMessage(string message, string receiverID)
        {
            Debug.WriteLine("RECEIVED MSG: " + message);
            Debug.WriteLine("RECEIVER ID: " + receiverID);

        }

        public void SendChatMessage(string who, string message)
        {
            string name = Context.User.Identity.Name;

            foreach (var connectionId in connections.GetConnections(who))
            {
                Clients.Client(connectionId).addChatMessage(name + ": " + message);
            }
        }

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;

            connections.Add(name, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;

            if (!connections.GetConnections(name).Contains(Context.ConnectionId))
            {
                connections.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }

        //public override Task OnConnected()
        //{
        //    string clientId = Context.ConnectionId;
        //    string data = clientId;

        //    Debug.WriteLine("ClientID: " + clientId);

        //    Clients.Caller.receiveMessage("ChatHub", data);

        //    return base.OnConnected();
        //}

        //[HubMethodName("connectHub")]
        //public void GetConnect(string username, string userid, string connectionid)
        //{
        //    string count = "";
        //    string msg = "";
        //    string list = "";
        //    try
        //    {
        //        //count = GetCount().ToString();
        //        //msg = updaterec(username, userid, connectionid);
        //        //list = GetUsers(username);
        //    }
        //    catch (Exception d)
        //    {
        //        msg = "DB Error " + d.Message;
        //    }
        //    var id = Context.ConnectionId;
        //    string[] Exceptional = new string[1];
        //    Exceptional[0] = id;
        //    Clients.Caller.receiveMessage("RU", msg, list);
        //    Clients.AllExcept(Exceptional).receiveMessage("NewConnection", username + " " + id, count);
        //}

        //public void Send(string name, string message, string connId)
        //{

        //    Debug.WriteLine("Name: " + name);
        //    Debug.WriteLine("message: " + message);
        //    Debug.WriteLine("connId: " + connId);
        //    Clients.Client(connId).appendNewMessage(name, message);
        //}

        //public DateTime GetCurrentDateTime()
        //{
        //    return DateTime.Now;
        //}
    }
}