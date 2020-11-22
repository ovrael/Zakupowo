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
    internal sealed class Configuration : DbMigrationsConfiguration<ShopApp.DAL.ShopContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ShopApp.DAL.ShopContext";
        }

        protected override void Seed(ShopContext context)
        {
            var users = new List<User>
            {
                new User{ Email="Seed@mail.com",
                    Login="SeedLogin",
                    EncryptedPassword="SeedPassword",
                    FirstName ="SeedFirstName",
                    LastName="SeedLastName",
                    Phone ="123456789"
                }
            };
            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();

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
                    CategoryDescription="Byłby po 3 ale bohater oddał jedno koledze"},
                new Category{ CategoryID = 6,CategoryName="Motoryzacja",
                    CategoryDescription="Byłby po 3 ale bohater oddał jedno koledze"},
                new Category{ CategoryID = 7, CategoryName="Sport i turystyka",
                    CategoryDescription="Byłby po 3 ale bohater oddał jedno koledze"},
                new Category{ CategoryID = 8,CategoryName="Zwierzęta",
                    CategoryDescription="Byłby po 3 ale bohater oddał jedno koledze"},
                new Category{ CategoryID = 9,CategoryName="Dla dziecka",
                    CategoryDescription="Byłby po 3 ale bohater oddał jedno koledze"},
                new Category{ CategoryID = 10,CategoryName="Sztuka",
                    CategoryDescription="Byłby po 3 ale bohater oddał jedno koledze"},
                new Category{ CategoryID = 11,CategoryName="Nieruchomości",
                    CategoryDescription="Byłby po 3 ale bohater oddał jedno koledze"},
                new Category{ CategoryID = 12,CategoryName="Zdrowie",
                    CategoryDescription="Byłby po 3 ale bohater oddał jedno koledze"},
                new Category{ CategoryID = 13,CategoryName="RTV i AGD",
                    CategoryDescription="Byłby po 3 ale bohater oddał jedno koledze"},
                new Category{ CategoryID = 14,CategoryName="Inne",
                    CategoryDescription="Byłby po 3 ale bohater oddał jedno koledze"}
            };
            categories.ForEach(u => context.Categories.Add(u));
            context.SaveChanges();

            var adresses = new List<ShippingAdress>
            {
                new ShippingAdress()
                {
                    Country = "Polska",
                    City = "Katowice",
                    Street = "Mariacka",
                    PremisesNumber = "33",
                    PostalCode = "40-220",
                    User = users[0]
                }
            };
            adresses.ForEach(a => context.ShippingAdresses.Add(a));
            context.SaveChanges();
        }
    }
}
