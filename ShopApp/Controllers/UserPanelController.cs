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
using ShopApp.Utility;
using System.Diagnostics;

namespace ShopApp.Controllers
{
    public class UserPanelController : Controller
    {
        private ShopContext db = new ShopContext();

        //USER REGISTRATION
        public ActionResult Register()
        {
            return View();
        }

        //POST: Register/Create
        [HttpPost]
        public ActionResult Register(FormCollection collection)
        {
            User user = new User()
            {
                FirstName = collection["FirstName"],
                LastName = collection["LastName"],
                Login = collection["Login"],
                EncryptedPassword = Cryptographing.Encrypt(collection["Password"]),
                Email = collection["Email"],
                BirthDate = DateTime.Parse(collection["BirthDate"]),
                CreationDate = DateTime.Now
            };


            //Debug.WriteLine("DANE USERA");
            //Debug.WriteLine(user.FirstName + " " + user.LastName + " " + user.Login + " " + user.EncryptedPassword + " " + user.Email);

            db.Users.Add(user);
            db.SaveChanges();
            ViewBag.Message = db.Users.ToList();
            return RedirectToAction("Account");
        }

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
                Login = "Migracja",
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
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Account()
        {
            if (Session["userId"] == null)
            {
                Response.Redirect("~/User/Login");
            }

            int userID = (int)Session["userId"];
            User showUser = db.Users.Where(u => u.UserID == userID).FirstOrDefault();

            AccountViewModel accountView = new AccountViewModel();
            accountView.Login = showUser.Login;
            accountView.FirstName = showUser.FirstName;
            accountView.LastName = showUser.LastName;
            accountView.Email = showUser.Email;
            accountView.BirthDate = showUser.BirthDate.ToString();
            accountView.PhoneNumber = showUser.Phone;

            return View(accountView);
        }

        public ActionResult AccountEdit()
        {
            if (Session["userId"] == null)
            {
                Response.Redirect("~/User/Login");
            }

            int userID = (int)Session["userId"];
            User showUser = db.Users.Where(u => u.UserID == userID).FirstOrDefault();

            AccountViewModel accountView = new AccountViewModel();
            accountView.Login = showUser.Login;
            accountView.FirstName = showUser.FirstName;
            accountView.LastName = showUser.LastName;
            accountView.Email = showUser.Email;
            accountView.BirthDate = showUser.BirthDate.ToString();
            accountView.PhoneNumber = showUser.Phone;

            return View(accountView);
        }

        [HttpPost]
        public ActionResult AccountEdit(FormCollection collection)
        {
            if (Session["userId"] == null)
            {
                Response.Redirect("~/User/Login");
            }

            int userID = (int)Session["userId"];
            User editUser = db.Users.Where(u => u.UserID == userID).FirstOrDefault();

            if (editUser != null)
            {
                Debug.WriteLine("ZMIENIAM INFO");
                Debug.WriteLine(editUser.showBasicInformation());

                string changedFirstName = collection["FirstName"].Trim();
                string changedLastName = collection["LastName"].Trim();
                string changedEmail = collection["Email"].Trim();
                string changedPhoneNumber = collection["PhoneNumber"].Trim();


                if (changedFirstName != editUser.FirstName && changedFirstName != null)
                    editUser.FirstName = changedFirstName;

                if (changedLastName != editUser.LastName && changedLastName != null)
                    editUser.LastName = changedLastName;

                if (changedEmail != editUser.Email && changedEmail != null)
                    editUser.Email = changedEmail;

                if (changedPhoneNumber != editUser.Phone && changedPhoneNumber != null)
                    editUser.Phone = changedPhoneNumber;

                Debug.WriteLine(editUser.showBasicInformation());

                db.Entry(editUser).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
            }

            return RedirectToAction("Account", "UserPanel");
        }

        public ActionResult AccountEditContact()
        {
            if (Session["userId"] == null)
            {
                Response.Redirect("~/User/Login");
            }

            int userID = (int)Session["userId"];
            User showUser = db.Users.Where(u => u.UserID == userID).FirstOrDefault();

            AccountViewModel accountView = new AccountViewModel();
            accountView.Country = showUser.Country;
            accountView.City = showUser.City;

            return View(accountView);
        }

        [HttpPost]
        public ActionResult AccountEditContact(FormCollection collection)
        {
            if (Session["userId"] == null)
            {
                Response.Redirect("~/User/Login");
            }

            int userID = (int)Session["userId"];
            User editUser = db.Users.Where(u => u.UserID == userID).FirstOrDefault();

            if (editUser != null)
            {
                Debug.WriteLine("ZMIENIAM INFO");
                Debug.WriteLine(editUser.showBasicInformation());

                string changedCountry = collection["Country"].Trim();
                string changedCity = collection["City"].Trim();


                if (changedCountry != editUser.Country && changedCountry != null)
                    editUser.Country = changedCountry;

                if (changedCity != editUser.City && changedCity != null)
                    editUser.City = changedCity;

                Debug.WriteLine(editUser.showBasicInformation());

                db.Entry(editUser).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
            }

            return RedirectToAction("Account", "UserPanel");
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
            User showUser = db.Users.ToList()[db.Users.ToList().Count - 1];

            AccountViewModel accountView = new AccountViewModel();

            accountView.Login = showUser.Login;
            accountView.FirstName = showUser.FirstName;
            accountView.LastName = showUser.LastName;
            accountView.Email = showUser.Email;
            accountView.BirthDate = showUser.BirthDate.ToString();

            return View(accountView);
        }
    }
}
