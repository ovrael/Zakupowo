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
using System.Security;
using System.Web.Configuration;
using System.Net;
using System.Web.Security;

namespace ShopApp.Controllers
{
    [Authorize]
    public class UserPanelController : Controller
    {
        private ShopContext db = new ShopContext();

       
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
        [Authorize]
        #region UserData 

        // VIEW WITH BASIC INFORMATION ABOUT USER
        public ActionResult Account()
        {
            User showUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();

            //AccountViewModel accountView = new AccountViewModel();
            //accountView.Login = showUser.Login;
            //accountView.Email = showUser.Email;
            //accountView.FirstName = showUser.FirstName;
            //accountView.LastName = showUser.LastName;
            //accountView.PhoneNumber = showUser.Phone;
            //accountView.BirthDate = showUser.BirthDate.ToString();
            //accountView.CreationDate = showUser.CreationDate.ToString();

            //As we pass the actual user the code above is unnecessary

            return View(showUser);
        }

        // VIEW WHERE USER CAN EDIT *BASIC* INFORMATION
        public ActionResult EditBasicInfo()
        {
            if (Session["userId"] == null)
            {
                return RedirectToAction("Login", "User");
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
        public ActionResult EditBasicInfo(FormCollection collection)
        {
            if (Session["userId"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            int userID = (int)Session["userId"];
            User editUser = db.Users.Where(u => u.UserID == userID).FirstOrDefault();

            if (editUser != null)
            {
                string changedFirstName = collection["FirstName"].Trim();
                string changedLastName = collection["LastName"].Trim();
                string changedEmail = collection["Email"].Trim();
                string changedLogin = collection["Login"].Trim();
                string changedPhoneNumber = collection["PhoneNumber"].Trim();


                if (changedFirstName != editUser.FirstName && changedFirstName != null)
                    editUser.FirstName = changedFirstName;

                if (changedLastName != editUser.LastName && changedLastName != null)
                    editUser.LastName = changedLastName;

                if (changedEmail != editUser.Email && changedEmail != null)
                    editUser.Email = changedEmail;

                if (changedPhoneNumber != editUser.Phone && changedPhoneNumber != null)
                    editUser.Phone = changedPhoneNumber;

                if (changedLogin != editUser.Login && changedLogin != null)
                    editUser.Login = changedLogin;

                db.Entry(editUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Account", "UserPanel");
        }

        // VIEW WHERE USER CAN EDIT SHIPPING ADRESSES
        public ActionResult ShippingAdresses()
        {
            if (Session["userId"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            int userID = (int)Session["userId"];
            User showUser = db.Users.Where(u => u.UserID == userID).FirstOrDefault();

            List<ShippingAdress> listaAdresow = showUser.ShippingAdresses.ToList();

            return View(listaAdresow);
        }

        [HttpPost]
        public ActionResult ShippingAdresses(FormCollection collection)
        {
            if (Session["userId"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            int userID = (int)Session["userId"];
            User editUser = db.Users.Where(u => u.UserID == userID).FirstOrDefault();

            if (editUser != null)
            {
                string changedCountry = collection["Country"].Trim();
                string changedCity = collection["City"].Trim();

                if (changedCountry != editUser.Country && changedCountry != null)
                    editUser.Country = changedCountry;

                if (changedCity != editUser.City && changedCity != null)
                    editUser.City = changedCity;

                db.Entry(editUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Account", "UserPanel");
        }

        public ActionResult AddShippingAdress()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddShippingAdress(FormCollection collection)
        {
            if (Session["userId"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            int userID = (int)Session["userId"];
            User editUser = db.Users.Where(u => u.UserID == userID).FirstOrDefault();

            string country = collection["Country"];
            string city = collection["city"];
            string street = collection["street"];
            string premisesNumber = collection["PremisesNumber"];
            string postalCode = collection["PostalCode"];

            ShippingAdress adress = new ShippingAdress
            {
                Country = country,
                City = city,
                Street = street,
                PremisesNumber = premisesNumber,
                PostalCode = postalCode,
                User = editUser
            };

            //using (var dataBase = new ShopContext())
            //{

            //    dataBase.Entry(adress).State = System.Data.Entity.EntityState.Modified;
            //    dataBase.Entry(editUser).State = System.Data.Entity.EntityState.Modified;

            //    editUser.ShippingAdresses.Add(adress);
            //    dataBase.SaveChanges();


            //    dataBase.ShippingAdresses.Add(adress);
            //    dataBase.SaveChanges();
            //}

            db.ShippingAdresses.Add(adress);
            db.SaveChanges();

            editUser.ShippingAdresses.Add(adress);
            db.SaveChanges();

            return RedirectToAction("ShippingAdresses", "UserPanel");
        }


        // VIEW WHERE USER CAN EDIT PASSWORD
        public ActionResult EditPassword()
        {
            if (Session["userId"] == null)
            {
                return RedirectToAction("Login", "User");
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
        public ActionResult EditPassword(FormCollection collection)
        {
            if (Session["userId"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            int userID = (int)Session["userId"];
            User editUser = db.Users.Where(u => u.UserID == userID).FirstOrDefault();

            if (editUser != null)
            {
                string encryptedOldPassword = Cryptographing.Encrypt(collection["OldPassword"].Trim());
                string encryptedNewPassword = Cryptographing.Encrypt(collection["NewPassword"].Trim());
                string encryptedNewPasswordValidation = Cryptographing.Encrypt(collection["NewPasswordValidation"].Trim());

                if (encryptedOldPassword != editUser.EncryptedPassword && encryptedOldPassword != null)
                {
                    if (encryptedNewPassword.Equals(encryptedNewPasswordValidation))
                    {
                        editUser.EncryptedPassword = encryptedNewPassword;

                        db.Entry(editUser).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Account", "UserPanel");
        }

        #endregion

        public ActionResult AddOffer()
        {
            List<Category> categoriesView = new List<Category>();

            using (ShopContext dataBase = new ShopContext())
            {
                //Adds new offer in offer table
                categoriesView = dataBase.Categories.ToList();
            }

            return View(categoriesView);
        }

        [HttpPost]
        public ActionResult AddOffer(FormCollection collection)
        {
            if (Session["userId"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            int userID = (int)Session["userId"];
            User editUser = db.Users.Where(u => u.UserID == userID).FirstOrDefault();

            Offer offer = new Offer
            {
                Title = collection["Name"],
                Description = collection["Description"],
                InStock = Convert.ToDouble(collection["Quantity"]),
                Price = Convert.ToDouble(collection["Price"]),
                //Category =
                User = editUser
            };

            //using (ShopContext dataBase = new ShopContext())
            //{
            //    //Adds new offer in offer table
            //    dataBase.Offers.Add(offer);
            //    dataBase.SaveChanges();

            //    //Adds the offer to offersList in User
            //    editUser.Offers.Add(offer);
            //    dataBase.SaveChanges();
            //}

            //Adds new offer in offer table
            db.Offers.Add(offer);
            db.SaveChanges();

            //Adds the offer to offersList in User
            editUser.Offers.Add(offer);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult OrderHistory()
        {
            return View();
        }

        public ActionResult Communicator()
        {
            return View();
        }
    }
}
