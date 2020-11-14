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
                new Category{ CategoryName=CategoryEnum.Elektronika,
                    CategoryDescription="Wszystkie te takie z prądem"},
                new Category{ CategoryName=CategoryEnum.ModaIUroda,
                    CategoryDescription="Koszulka z napisem konstytucja"},
                new Category{ CategoryName=CategoryEnum.Meble,
                    CategoryDescription="Szafka prawie wisząca"},
                new Category{ CategoryName=CategoryEnum.SlawekPo2Piwach,
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
