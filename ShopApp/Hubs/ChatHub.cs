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

                var userConnections = db.UserConnections.Where(u => u.UserName == receiver.Login).ToList();

                if (userConnections != null && userConnections.Count > 0)
                {
                    string imageURL = sender.AvatarImage.PathToFile;

                    foreach (var connection in userConnections)
                    {
                        string userConnectionID = connection.ConnectionID;
                        Clients.Client(userConnectionID).receiveMessage(message, sender.UserID, sender.Login, imageURL);

                    }

                    //Debug.WriteLine("------------ WYSYŁAM WIADOMOŚĆ DO " + userConnectionID);
                    //Debug.WriteLine("------------ MÓJ CONNECTION ID: " + Context.ConnectionId);
                    //Clients.User(userConnectionID).receiveMessage(message, sender.UserID, sender.Login, imageURL);
                    //Clients.User(receiver.Login).receiveMessage(message, sender.UserID, sender.Login, imageURL);

                    //Clients.Client(receiver.Login).receiveMessage(message, sender.UserID, sender.Login, imageURL);

                    //Clients.All.receiveMessage(message, sender.UserID, sender.Login, imageURL);
                }

                //if (userConnections.ContainsKey(receiver.Login))
                //{
                //    string userConnectionID = userConnections[receiver.Login];
                //    string imageURL = sender.AvatarImage.PathToFile;

                //    Clients.Client(userConnectionID).receiveMessage(message, sender.UserID, sender.Login, imageURL);
                //}
            }
        }

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;
            string connectionID = Context.ConnectionId;

            Debug.WriteLine("------------------------ OnConnected! Podłączono: " + name);
            Trace.TraceError("------------------------ OnConnected! Podłączono: " + name + "\tConnectionID: " + connectionID);

            using (var db = new DAL.ShopContext())
            {
                var oldConnection = db.UserConnections.Where(u => u.UserName == name).FirstOrDefault();
                var newConnection = new UserConnection() { UserName = name, ConnectionID = connectionID };

                if (oldConnection != null && oldConnection.UserConnectionID != newConnection.UserConnectionID)
                {
                    Trace.TraceError("------------------------ Dodaje do bazy połączenie: " + newConnection.UserName + "\tConnectionID: " + newConnection.ConnectionID);
                    db.UserConnections.Add(newConnection);
                    db.SaveChanges();
                }

                if (oldConnection == null)
                {
                    Trace.TraceError("------------------------ Dodaje do bazy połączenie: " + newConnection.UserName + "\tConnectionID: " + newConnection.ConnectionID);
                    db.UserConnections.Add(newConnection);
                    db.SaveChanges();
                }

                //if (oldConnection == null)
                //{
                //    Trace.TraceError("------------------------ Dodaje do bazy połączenie: " + newConnection.UserName + "\tConnectionID: " + newConnection.ConnectionID);
                //    db.UserConnections.Add(newConnection);
                //    db.SaveChanges();
                //}
                //else
                //{
                //    Trace.TraceError("------------------------ Zmieniam STARE połączenie: " + oldConnection.UserName + "\tConnectionID: " + oldConnection.ConnectionID);
                //    Trace.TraceError("------------------------ Na NOWE połączenie: " + newConnection.UserName + "\tConnectionID: " + newConnection.ConnectionID);
                //    oldConnection = newConnection;
                //    db.SaveChanges();
                //}
            }

            //if (!userConnections.ContainsKey(name))
            //{
            //    userConnections.Add(name, connectionID);
            //}

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;
            string connectionID = Context.ConnectionId;

            Debug.WriteLine("------------------------ OnDisconnected! Odłączono: " + name);
            Trace.TraceError("------------------------ OnDisconnected! Odłączono : " + name);

            using (var db = new DAL.ShopContext())
            {

                var oldConnection = db.UserConnections.Where(u => u.UserName == name && u.ConnectionID == connectionID).FirstOrDefault();
                if (oldConnection != null)
                {
                    Trace.TraceError("------------------------ Usuwam z bazy połączenie: " + oldConnection.UserName + "\tConnectionID: " + oldConnection.ConnectionID);
                    db.UserConnections.Remove(oldConnection);
                    db.SaveChanges();
                }
                //var oldConnections = db.UserConnections.Where(u => u.UserName == name).ToList();
                //if (oldConnections.Count > 0 && oldConnections != null)
                //{
                //    foreach (var connection in oldConnections)
                //    {
                //        Trace.TraceError("------------------------ Usuwam z bazy połączenie: " + connection.UserName + "\tConnectionID: " + connection.ConnectionID);
                //        db.UserConnections.Remove(connection);
                //        db.SaveChanges();
                //    }
                //}
            }

            //if (userConnections.ContainsKey(name))
            //{
            //    userConnections.Remove(name);
            //}

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;
            string connectionID = Context.ConnectionId;

            Debug.WriteLine("------------------------ OnReconnected! Podłączono PONOWNIE: " + name);
            Trace.TraceError("------------------------ OnReconnected! Podłączono PONOWNIE: " + name);

            using (var db = new DAL.ShopContext())
            {
                var oldConnection = db.UserConnections.Where(u => u.UserName == name).FirstOrDefault();
                var newConnection = new UserConnection() { UserName = name, ConnectionID = connectionID };

                Trace.TraceError("------------------------ Dodaje do bazy połączenie: " + newConnection.UserName + "\tConnectionID: " + newConnection.ConnectionID);
                db.UserConnections.Add(newConnection);
                db.SaveChanges();

                //var oldConnection = db.UserConnections.Where(u => u.UserName == name).FirstOrDefault();
                //var newConnection = new UserConnection() { UserName = name, ConnectionID = connectionID };

                //if (oldConnection == null)
                //{
                //    Trace.TraceError("------------------------ Dodaje do bazy połączenie: " + newConnection.UserName + "\tConnectionID: " + newConnection.ConnectionID);
                //    db.UserConnections.Add(newConnection);
                //    db.SaveChanges();
                //}
                //else
                //{
                //    Trace.TraceError("------------------------ Zmieniam STARE połączenie: " + oldConnection.UserName + "\tConnectionID: " + oldConnection.ConnectionID);
                //    Trace.TraceError("------------------------ Na NOWE połączenie: " + newConnection.UserName + "\tConnectionID: " + newConnection.ConnectionID);
                //    oldConnection = newConnection;
                //    db.SaveChanges();
                //}
            }

            //if (!userConnections.ContainsKey(name))
            //{
            //    userConnections.Add(name, connectionID);
            //}

            return base.OnReconnected();
        }
    }
}