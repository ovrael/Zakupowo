using ShopApp.Models;
using ShopApp.ViewModels;
using ShopApp.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopApp.DAL;
using Microsoft.Ajax.Utilities;
using Antlr.Runtime.Tree;

namespace ShopApp.Controllers
{
    public class UserPanelController : Controller
    {
        private ShopContext db = new ShopContext();


        // GET: User
        public ActionResult Index()
        {
            return View(db.Offers);
        }
        public ActionResult AccountAddProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AccountAddProduct(FormCollection collection)
        {
            Offer Oferta = new Offer
            {
                Title = collection["product_name"],
                Description = collection["product_name_fr"],
                InStock = Convert.ToDouble(collection["available_quantity"]),
                Price = Convert.ToDouble(collection["product_price"])
            };
            db.Users.Add(new Models.User
            {
                Email = "Toniezle@Migracje.com",
                Login ="Migracja",
                EncryptedPassword = "TakO",
                FirstName = "Wywala",
                LastName = "Seeda:D"
            }
            );
            db.SaveChanges();
            Oferta.User = db.Users.Where(i => i.UserID == 1).First();//DO PIERWSZEGO SPRINTU WSZYSTKO WLATUJE DO DEFAULTOWEGO USERA
            db.Offers.Add(Oferta);
            db.SaveChanges();
            /*
            int ID;
            foreach (var item in collection["product_categorie"])
            {
                ID = Convert.ToInt32(item);
                //TU SIE PSUJE STRASZNIE FEST
                (db.Categories.Where(i => i.CategoryID == ID).First())
                    .Offers.Add(db.Offers.Where(x => x.OfferID == Oferta.OfferID).First());
                Oferta.Categories.Add(db.Categories.Where(i => i.CategoryID == ID).First());
            }
            */
            
            db.SaveChanges();
            db.Users.Where(i => i.UserID == 1).First().Offers.Add(Oferta);
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        public ActionResult Account()
        {
            AccountViewModel accountView = new AccountViewModel();
            accountView.FirstName = UserCache.CachedUser.FirstName;
            accountView.Surname = UserCache.CachedUser.LastName;
            accountView.Email = UserCache.CachedUser.Email;
            accountView.PhoneNumber = "+48 111 222 333";
            accountView.City = "Katowice";
            accountView.Street = "Mickiewicza";
            accountView.StreetNumber = 45;
            accountView.Postcode = "40-008";
            accountView.BirthDate = "21 kwietnia 1999";
            accountView.Country = "Polska";

            return View(accountView);
        }
        
        public ActionResult AccountEdit()
        {

            AccountViewModel accountView = new AccountViewModel();
            accountView.FirstName = UserCache.CachedUser.FirstName;
            accountView.Surname = UserCache.CachedUser.LastName;
            accountView.Email = UserCache.CachedUser.Email;
            accountView.PhoneNumber = "+48 111 222 333";
            accountView.City = "Katowice";
            accountView.Street = "Mickiewicza";
            accountView.StreetNumber = 45;
            accountView.Postcode = "40-008";
            accountView.BirthDate = "21 kwietnia 1999";
            accountView.Country = "Polska";

            return View(accountView);
        }
        public ActionResult AccountEditContact()
        {

            AccountViewModel accountView = new AccountViewModel();
            accountView.FirstName = UserCache.CachedUser.FirstName;
            accountView.Surname = UserCache.CachedUser.LastName;
            accountView.Email = UserCache.CachedUser.Email;
            accountView.PhoneNumber = "+48 111 222 333";
            accountView.City = "Katowice";
            accountView.Street = "Mickiewicza";
            accountView.StreetNumber = 45;
            accountView.Postcode = "40-008";
            accountView.BirthDate = "21 kwietnia 1999";
            accountView.Country = "Polska";

            return View(accountView);
        }
        public ActionResult AccountOrderHistory()
        {
            return View();
        }
        
        public ActionResult AccountMessage()
        {
            return View();
        }
        public ActionResult AccountEditPassword()
        {
            AccountViewModel accountView = new AccountViewModel();
            accountView.FirstName = UserCache.CachedUser.FirstName;
            accountView.Surname = UserCache.CachedUser.LastName;
            accountView.Email = UserCache.CachedUser.Email;
            accountView.PhoneNumber = "+48 111 222 333";
            accountView.City = "Katowice";
            accountView.Street = "Mickiewicza";
            accountView.StreetNumber = 45;
            accountView.Postcode = "40-008";
            accountView.BirthDate = "21 kwietnia 1999";
            accountView.Country = "Polska";

            return View(accountView);
        }
      
      
        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult EditAccount(FormCollection viewModel)
        {
            UserCache.CachedUser.FirstName = viewModel["FirstName"];
            UserCache.CachedUser.LastName = viewModel["SurName"];
            UserCache.CachedUser.Email = viewModel["Email"];
            
            AccountViewModel accountView = new AccountViewModel();
            accountView.FirstName = UserCache.CachedUser.FirstName;
            accountView.Surname = UserCache.CachedUser.LastName;
            accountView.Email = UserCache.CachedUser.Email;
            accountView.PhoneNumber = "+48 111 222 333";
            accountView.City = "Katowice";
            accountView.Street = "Mickiewicza";
            accountView.StreetNumber = 45;
            accountView.Postcode = "40-008";
            accountView.BirthDate = "21 kwietnia 1999";
            accountView.Country = "Polska";
            return View("AccountEdit", accountView);
        }
        [HttpPost]
        public ActionResult AccountEditContact(FormCollection viewModel)
        {
            
            UserCache.CachedUser.Email = viewModel["Email"];

            AccountViewModel accountView = new AccountViewModel();
            accountView.FirstName = UserCache.CachedUser.FirstName;
            accountView.Surname = UserCache.CachedUser.LastName;
            accountView.Email = UserCache.CachedUser.Email;
            accountView.PhoneNumber = "+48 111 222 333";
            accountView.City = "Katowice";
            accountView.Street = "Mickiewicza";
            accountView.StreetNumber = 45;
            accountView.Postcode = "40-008";
            accountView.BirthDate = "21 kwietnia 1999";
            accountView.Country = "Polska";
            return View("AccountEditContact", accountView);
        }
        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
