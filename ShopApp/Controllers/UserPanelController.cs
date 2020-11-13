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
            User user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();
            Offer Oferta = new Offer
            {
                Title = collection["product_name"],
                Description = collection["product_name_fr"],
                InStock = Convert.ToDouble(collection["available_quantity"]),
                Price = Convert.ToDouble(collection["product_price"]),


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
            };
            DataBase.AddToDatabase(Oferta, user);
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        #region UserData 

        // VIEW WITH BASIC INFORMATION ABOUT USER
        public ActionResult Account()
        {
            User showUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();

            int userID = (int)Session["userId"];
            User showUser = db.Users.Where(u => u.UserID == userID).FirstOrDefault();

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

            return View(showUser);
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
                string changedPhoneNumber = collection["Phone"].Trim();


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

            return RedirectToAction("EditBasicInfo", "UserPanel");
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

            return View(showUser);
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
            ShippingAdress shippingAdress;

            if (editUser != null)
            {
                int adressNumber = int.Parse(collection["AdressNumber"].Trim());
                shippingAdress = editUser.ShippingAdresses.ToList()[adressNumber];

                string changedCountry = collection["Country"].Trim();
                string changedCity = collection["City"].Trim();
                string changedStreet = collection["Street"].Trim();
                string changedPremisesNumber = collection["PremisesNumber"].Trim();
                string changedPostalCode = collection["PostalCode"].Trim();

                if (changedCountry != shippingAdress.Country && changedCountry != null)
                    shippingAdress.Country = changedCountry;

                if (changedCity != shippingAdress.City && changedCity != null)
                    shippingAdress.City = changedCity;

                if (changedStreet != shippingAdress.Street && changedStreet != null)
                    shippingAdress.Street = changedStreet;

                if (changedPremisesNumber != shippingAdress.PremisesNumber && changedPremisesNumber != null)
                    shippingAdress.PremisesNumber = changedPremisesNumber;

                if (changedPostalCode != shippingAdress.PostalCode && changedPostalCode != null)
                    shippingAdress.PostalCode = changedPostalCode;


                editUser.ShippingAdresses.ToList()[adressNumber] = shippingAdress;

                db.Entry(editUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("ShippingAdresses", "UserPanel");
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

        public ActionResult DeleteShippingAdress(int? adressNumber)
        {
            if (Session["userId"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            if (adressNumber == null)
                return RedirectToAction("ShippingAdresses", "UserPanel");


            int userID = (int)Session["userId"];
            User editUser = db.Users.Where(u => u.UserID == userID).FirstOrDefault();

            List<ShippingAdress> lista = db.ShippingAdresses.Where(u => u.User.UserID == userID).ToList();

            ShippingAdress adressToRemove = db.ShippingAdresses.Where(u => u.User.UserID == userID).ToList()[(int)adressNumber];

            db.ShippingAdresses.Remove(adressToRemove);
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

            return View(showUser);
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
