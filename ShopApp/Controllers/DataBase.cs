using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using ShopApp.DAL;
using ShopApp.Models;
using ShopApp.Utility;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ShopApp.Controllers
{
    public static class DataBase
    {
        private static ShopContext db = new ShopContext();
        public static void AddToDatabase(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }
        public static void AddToDatabase(Offer oferta, User user)
        {
            db.Offers.Add(oferta);
            db.SaveChanges();
            oferta.User = user;
            

            db.SaveChanges();
            db.Users.Where(i => i.UserID == 1).First().Offers.Add(oferta);
            db.SaveChanges();
        }
}
}