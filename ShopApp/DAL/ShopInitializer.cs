using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopApp.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Configuration;
using ShopApp.DAL;

namespace ShopApp.DAL
{
    public class ShopInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ShopContext>
    {
        protected override void Seed(ShopContext context)
        {
            var usrs = new List<User>
            {
                new User{ Email="Mail@SeedUsr.com",
                    Login="LoginSeedUsr",
                    EncryptedPassword="PasswwordSeedUsr",
                    FirstName ="Imie",
                    LastName="Nazwisko",
                    Phone ="123456789"
                }
            };
            usrs.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }

    }
}