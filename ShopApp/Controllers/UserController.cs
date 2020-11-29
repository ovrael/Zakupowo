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
            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            else
                return View();
        }

        //POST: Register/Create
        [HttpPost]
        public async Task<ActionResult> Register(FormCollection collection)
        {
            string email = collection["Email"].Trim();
            string login = collection["Login"].Trim();
            User tmpEmailUser = db.Users.Where(u => u.Email == email).FirstOrDefault();
            User tmpLoginUser = db.Users.Where(u => u.Login == login).FirstOrDefault();

            bool properDate = DateTime.TryParse(collection["BirthDate"], out DateTime dataUrodzenia);
            bool properAge = Utilities.CheckRegistrationAge(dataUrodzenia);
            bool uniqueEmail = tmpEmailUser is null; // If user with given EMAIL doesn't exist returns true that allows to register, works like "tmpEmailUser is null ? true : null"
            bool uniqueLogin = tmpLoginUser is null; // If user with given LOGIN doesn't exist returns true that allows to register, works like "tmpLoginUser is null ? true : null"

            if (!properDate) ViewBag.DateMessage = "Invalid date.";
            if (!properAge) ViewBag.AgeMessage = "You must be at least 13 years old.";
            if (!uniqueEmail) ViewBag.EmailMessage = "Account with that email already exists!";
            if (!uniqueLogin) ViewBag.LoginMessage = "Account with that login already exists!";

            if (ModelState.IsValid && properDate && properAge && uniqueEmail && uniqueLogin)
            {
                User user = new User()
                {
                    FirstName = collection["FirstName"].Trim(),
                    LastName = collection["LastName"].Trim(),
                    Login = login,
                    EncryptedPassword = Cryptographing.Encrypt(collection["Password"]),
                    Email = email,
                    BirthDate = dataUrodzenia,
                    CreationDate = DateTime.Now,
                    IsActivated = false
                };

                db.Users.Add(user);
                db.SaveChanges();
                var bucket = new Bucket
                {
                    User = db.Users.Where(i => i.Login == user.Login).First()
                };
                db.Buckets.Add(bucket);
                db.SaveChanges();
                
                Task.Run(() => EmailManager.SendEmailAsync(EmailManager.EmailType.Registration, user.FirstName, user.LastName, user.Email));
                return RedirectToAction("Login");
            }
            return View();
        }


        public ActionResult ActivateAccount()
        {
            string userEmail = TempData["email"].ToString();
            User editUser = db.Users.Where(u => u.Email.Equals(userEmail)).FirstOrDefault();

            if (editUser != null)
            {
                editUser.IsActivated = true;
                db.SaveChanges();

                return View(editUser);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult ConfirmRegistration(string email)
        {
            string userEmail = EmailManager.DecryptEmail(email);
            TempData["email"] = userEmail;

            User editUser = db.Users.Where(u => u.Email.Equals(userEmail)).FirstOrDefault();

            if (editUser != null)
                return RedirectToAction("ActivateAccount", "User");
            else
                return RedirectToAction("Index", "Home");
        }

        //Login methods
        public ActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            else
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
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }
    }
}
