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


namespace ShopApp.Controllers
{
    public class UserController : Controller
    {
        private ShopContext db = new ShopContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var email = collection["Email"];
            var password = collection["Password"];


            Cryptographing.Encrypt(collection["Password"]);
           
            ViewBag.Message = db.Users.ToList();
            return RedirectToAction("~/Userpanel/Account");
        }

        //  GET: Register

        public ActionResult Usr()
        {
            return View();
        }

        //  GET: Register/Create
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
                Login = "TEST_LOGIN",
                EncryptedPassword = Cryptographing.Encrypt(collection["Password"]),
                Email = collection["Email"],
                BirthDate = DateTime.UtcNow
            };


            //Debug.WriteLine("DANE USERA");
            //Debug.WriteLine(user.FirstName + " " + user.LastName + " " + user.Login + " " + user.EncryptedPassword + " " + user.Email);

            db.Users.Add(user);
            db.SaveChanges();
            ViewBag.Message = db.Users.ToList();
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
