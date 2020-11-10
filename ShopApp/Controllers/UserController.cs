using ShopApp.DAL;
using ShopApp.Models;
using ShopApp.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace ShopApp.Controllers
{
    public class UserController : Controller
    {
        private ShopContext db = new ShopContext();

        // VIEW WHERE USER CAN CREATE ACCOUNT
        public ActionResult Register()
        {
            return View();
        }

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

            db.Users.Add(user);
            db.SaveChanges();
            ViewBag.Message = db.Users.ToList();
            return RedirectToAction("Account", "UserPanel");
        }

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
            return RedirectToAction("Account","userpanel");
        }


        //Login methods
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            
            var email = collection["Email"];
            var password = Cryptographing.Encrypt(collection["EncryptedPassword"]);
            var user = db.Users.Where(x => x.Email == email && x.EncryptedPassword == password).First();

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Login, false);
                return RedirectToAction("Index", "Home");
            }
            /*
            if (userDetail == null)
            {
                ViewBag.Error = "Nieprawidłowe dane logowania";
                return View("Login");
            }
            else
            {
                int userId = userDetail.UserID;
                Session["userId"] = userId;
                return RedirectToAction("Index", "Home");
            }
            */
            return RedirectToAction("Login");


        }
        //Logout method 

        public ActionResult Logout()
        {
            // Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }
        #region NotYetUsedActions
        // GET: Register/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Register/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Register/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                //TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //  GET: Register/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Register/Delete/5
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
        #endregion
    }
}
