using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopApp.Models;
using System.Data.Entity;

namespace ShopApp.DAL
{
        public class ShopInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ShopContext>
        {
        /*
            protected override void Seed(ShopContext context)
            {
                var Users = new List<User>
            {
          
           * Seed determines which records go every time the base is being initialized
                    new User{Login="Carson",Password="Alexander",Email="Carson@Alexander.com"},
                    new User{Login="Alonso",Password="Meredith",Email="Alonso@Meredith.com"},
                    
            };

                Users.ForEach(s => context.Users.Add(s));
                context.SaveChanges();
             */
        }
}