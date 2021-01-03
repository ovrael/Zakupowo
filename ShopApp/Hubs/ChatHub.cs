using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                    SentTime = DateTime.Now,
                    IsRead = false
                };

                db.Messages.Add(sendMessage);
                db.SaveChanges();

                receiver.ReceivedMessages.Add(sendMessage);
                db.SaveChanges();

                sender.SentMessages.Add(sendMessage);
                db.SaveChanges();

                var userConnections = db.UserConnections.Where(u => u.UserName == receiver.Login).ToList();

                if (userConnections != null && userConnections.Count > 0)
                {
                    string imageURL = sender.AvatarImage.PathToFile;

                    foreach (var connection in userConnections)
                    {
                        string userConnectionID = connection.ConnectionID;
                        Clients.Client(userConnectionID).receiveMessage(message, sender.UserID, sender.Login, imageURL);

                    }
                }
            }
        }

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;
            string connectionID = Context.ConnectionId;

            //Debug.WriteLine("------------------------ OnConnected! Podłączono: " + name);
            //Trace.TraceError("------------------------ OnConnected! Podłączono: " + name + "\tConnectionID: " + connectionID);\

            using (var db = new DAL.ShopContext())
            {
                var connectionInDatabase = db.UserConnections.Where(c => c.UserName == name && c.ConnectionID == connectionID).FirstOrDefault();

                if (connectionInDatabase == null)
                {
                    var connection = new UserConnection() { UserName = name, ConnectionID = connectionID, CreationDate = DateTime.UtcNow };
                    //Trace.TraceError("------------------------ Dodaje do bazy połączenie dla: " + connection.UserName + "\tConnectionID: " + connection.ConnectionID);
                    db.UserConnections.Add(connection);
                    db.SaveChanges();
                }

                var yesterdayDateUtc = DateTime.UtcNow.AddDays(-1);
                var oldConnections = db.UserConnections.Where(c => c.UserName == name && c.CreationDate.CompareTo(yesterdayDateUtc) == -1).ToList();

                foreach (var connection in oldConnections)
                {
                    db.UserConnections.Remove(connection);
                    db.SaveChanges();
                }


            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;
            string connectionID = Context.ConnectionId;

            //Debug.WriteLine("------------------------ OnDisconnected! Odłączono: " + name);
            //Trace.TraceError("------------------------ OnDisconnected! Odłączono : " + name);

            using (var db = new DAL.ShopContext())
            {

                var connection = db.UserConnections.Where(u => u.UserName == name && u.ConnectionID == connectionID).FirstOrDefault();
                if (connection != null)
                {
                    //Trace.TraceError("------------------------ Usuwam z bazy połączenie dla: " + connection.UserName + "\tConnectionID: " + connection.ConnectionID);
                    db.UserConnections.Remove(connection);
                    db.SaveChanges();
                }
            }

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;
            string connectionID = Context.ConnectionId;

            //Debug.WriteLine("------------------------ OnReconnected! Podłączono PONOWNIE: " + name);
            //Trace.TraceError("------------------------ OnReconnected! Podłączono PONOWNIE: " + name);

            using (var db = new DAL.ShopContext())
            {
                var connectionInDatabase = db.UserConnections.Where(c => c.UserName == name && c.ConnectionID == connectionID).FirstOrDefault();

                if (connectionInDatabase == null)
                {
                    var connection = new UserConnection() { UserName = name, ConnectionID = connectionID, CreationDate = DateTime.UtcNow };
                    //Trace.TraceError("------------------------ Łącze się ponownie i doddaje połączenie dla: " + connection.UserName + "\tConnectionID: " + connection.ConnectionID);
                    db.UserConnections.Add(connection);
                    db.SaveChanges();
                }
            }

            return base.OnReconnected();
        }
    }
}