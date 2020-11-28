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
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace ShopApp.Controllers
{
    public class UserController : Controller
    {

        private ShopContext db = new ShopContext();

        //USER REGISTRATION
        public ActionResult Register()
        {
            return View();
        }

        //POST: Register/Create
        [HttpPost]
        public async Task<ActionResult> Register(FormCollection collection)
        {
            if (ModelState.IsValid && DateTime.TryParse(collection["BirthDate"], out DateTime DataUrodzenia))
            {
                User user = new User()
                {
                    FirstName = collection["FirstName"].Trim(),
                    LastName = collection["LastName"].Trim(),
                    Login = collection["Login"].Trim(),
                    EncryptedPassword = Cryptographing.Encrypt(collection["Password"]),
                    Email = collection["Email"].Trim(),
                    BirthDate = DateTime.Parse(collection["BirthDate"]),
                    CreationDate = DateTime.Now
                };
                db.Users.Add(user);
                db.SaveChanges();
                await EmailManager.SendEmailAsync(EmailManager.EmailType.Registration, user.FirstName, user.LastName, user.Email);
                return RedirectToAction("Account", "userpanel");
            }
            return RedirectToAction("Register");
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

            var user = db.Users.Where(x => x.Email == email && x.EncryptedPassword == password).SingleOrDefault();
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Login, (collection["rememberMeInput"] == "rememberMe" ? true : false)); //TODO ISCHECKED
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Nieprawidłowe dane logowania";
            return View();
        }
        //Logout method 

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }
    }
}
