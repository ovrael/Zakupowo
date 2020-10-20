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

        protected override void Seed(ShopContext context)
        {
            //Seed determines which records go every time the base is being initialized

            var users = new List<User>
            {
                    new User{FirstName="Imie",  LastName="Nazwisko"}
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();


            var categories = new List<Category>
            {
                    new Category{Name="Phones"}
            };

            categories.ForEach(c => context.Categories.Add(c));
            context.SaveChanges();


            var products = new List<Product>
            {
                    new Product{ProductID=1, Category="Phones", Name="Apple iPhone 12", Weight=735}
            };

            products.ForEach(p => context.Products.Add(p));
            context.SaveChanges();


            var offers = new List<Offer>
            {
                    new Offer{ProductID=1,  UserID=1, Title="NEW IPHONE XII BUY NOW!!", Description="HERE IT IS NEW IPHONE XD!", Price=749}
            };

            offers.ForEach(o => context.Offers.Add(o));
            context.SaveChanges();

        }
    }
}