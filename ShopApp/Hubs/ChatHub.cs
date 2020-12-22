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
    [Authorize]
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        private readonly static Dictionary<string, string> userConnections = new Dictionary<string, string>();

        public void SendMessage(string message, string receiverID)
        {
            var userLogin = Context.User.Identity.Name;

            int.TryParse(receiverID, out int int_receiverID);

            using (var db = new DAL.ShopContext())
            {
                User sender = db.Users.Where(u => u.Login == userLogin).FirstOrDefault();
                User receiver = db.Users.Where(u => u.UserID == int_receiverID).FirstOrDefault();

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

                if (userConnections.ContainsKey(receiver.Login))
                {
                    string userConnectionID = userConnections[receiver.Login];
                    string imageURL = sender.AvatarImage.PathToFile;

                    Clients.Client(userConnectionID).receiveMessage(message, sender.UserID, sender.Login, imageURL);
                }
            }
        }

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;
            string connectionID = Context.ConnectionId;

            if (!userConnections.ContainsKey(name))
            {
                userConnections.Add(name, connectionID);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            if (userConnections.ContainsKey(name))
            {
                userConnections.Remove(name);
            }

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;
            string connectionID = Context.ConnectionId;

            if (!userConnections.ContainsKey(name))
            {
                userConnections.Add(name, connectionID);
            }

            return base.OnReconnected();
        }
    }
}