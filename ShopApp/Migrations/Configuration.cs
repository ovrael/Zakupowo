using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopApp.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;
using System.Configuration;
using ShopApp.DAL;

namespace ShopApp.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<ShopApp.DAL.ShopContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ShopApp.DAL.ShopContext";
        }

        protected override void Seed(ShopContext context)
        {
            User admin = context.Users.Where(u => u.Login == "Zakupowo").FirstOrDefault();

            if (admin == null)
            {
                var user = new User()
                {
                    UserID = 1,
                    Email = "Zakupowo2020@gmail.com",
                    Login = "Zakupowo",
                    EncryptedPassword = Utility.Cryptographing.Encrypt("Zakupowo2020$$$"),
                    FirstName = "Zakupowo",
                    LastName = "Administration",
                    CreationDate = DateTime.Now,
                    Bucket = new Bucket(),
                    Order = new Order()
                };

                var avatarImage = new AvatarImage()
                {
                    AvatarImageID = 1,
                    PathToFile = "../../App_Files/Images/UserAvatars/DefaultAvatar.jpg",
                    User = user
                };

                context.AvatarImages.Add(avatarImage);
                context.SaveChanges();

                context.Users.Add(user);
                context.SaveChanges();
            }


            var categories = new List<Category>
            {
                new Category{ CategoryID = 1, CategoryName="Elektronika",
                    CategoryDescription="Wszystkie te takie z prądem"},
                new Category{ CategoryID = 2,CategoryName="Moda",
                    CategoryDescription="Koszulka z napisem konstytucja"},
                new Category{ CategoryID = 3,CategoryName="Ogród",
                    CategoryDescription="Szafka prawie wisząca"},
                new Category{ CategoryID = 4,CategoryName="Dom",
                    CategoryDescription="Byłby po 3 ale bohater oddał jedno koledze"},
                new Category{ CategoryID = 5,CategoryName="Supermarket",
                    CategoryDescription="Albo hipermarket jak kto woli"},
                new Category{ CategoryID = 6,CategoryName="Motoryzacja",
                    CategoryDescription="Mrrrrrr zaszpanuj swoim jeden dziewięć TDI"},
                new Category{ CategoryID = 7, CategoryName="Sport i turystyka",
                    CategoryDescription="Sport to zdrowie, a zdrowie to sam zobacz co"},
                new Category{ CategoryID = 8,CategoryName="Zwierzęta",
                    CategoryDescription="Ludzi tu nie kupisz."},
                new Category{ CategoryID = 9,CategoryName="Dla dziecka",
                    CategoryDescription="Gerberki dla dorosłych to byłoby coś."},
                new Category{ CategoryID = 10,CategoryName="Sztuka",
                    CategoryDescription="Kto ją zrozumie?"},
                new Category{ CategoryID = 11,CategoryName="Nieruchomości",
                    CategoryDescription="Nie mieszkaj pod mostem!"},
                new Category{ CategoryID = 12,CategoryName="Zdrowie",
                    CategoryDescription="Bo zdrowie jest najważniejsze!"},
                new Category{ CategoryID = 13,CategoryName="RTV i AGD",
                    CategoryDescription="AGD i RTV RTV i AGD"},
                new Category{ CategoryID = 14,CategoryName="Inne",
                    CategoryDescription="Wszystkie takie co nie mieszczą się w innych"}
            };
            categories.ForEach(u => context.Categories.AddOrUpdate(u));
            context.SaveChanges();
        }
    }
}
