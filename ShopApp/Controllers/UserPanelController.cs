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

        #region UserData 

        // VIEW WITH BASIC INFORMATION ABOUT USER
        public ActionResult Account()
        {
            if (Session["userId"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            int userID = (int)Session["userId"];
            User showUser = db.Users.Where(u => u.UserID == userID).FirstOrDefault();

            AccountViewModel accountView = new AccountViewModel();
            accountView.Login = showUser.Login;
            accountView.Email = showUser.Email;
            accountView.FirstName = showUser.FirstName;
            accountView.LastName = showUser.LastName;
            accountView.PhoneNumber = showUser.Phone;
            accountView.BirthDate = showUser.BirthDate.ToString();
            accountView.CreationDate = showUser.CreationDate.ToString();

            return View(accountView);
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

            AccountViewModel accountView = new AccountViewModel();

            List<ShippingAdressViewModel> listaAdresow = new List<ShippingAdressViewModel>()
            {
                new ShippingAdressViewModel()
                {
                    Country = "Poland",
                    City = "Katowice",
                    Street = "3 Maja",
                    PremisesNumber = "25",
                    PostalCode = "40-215"
                },
                new ShippingAdressViewModel()
                {
                    Country = "Poland",
                    City = "Warszawa",
                    Street = "Piłsudskiego",
                    PremisesNumber = "4",
                    PostalCode = "40-452"
                },
                new ShippingAdressViewModel()
                {
                    Country = "Poland",
                    City = "Sosnowiec",
                    Street = "Niepodległości",
                    PremisesNumber = "7",
                    PostalCode = "40-218"
                },
                new ShippingAdressViewModel()
                {
                    Country = "Poland",
                    City = "Katowice",
                    Street = "Mariacka",
                    PremisesNumber = "13",
                    PostalCode = "40-215"
                }
            };

            accountView.ShippingAdresses = listaAdresow;

            //accountView.Country = showUser.Country;
            //accountView.City = showUser.City;

            return View(accountView);
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

        public ActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(FormCollection collection)
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
