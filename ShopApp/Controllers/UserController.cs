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
                Debug.WriteLine(collection["rememberMe"]);
                FormsAuthentication.SetAuthCookie(user.Login,false)); //TODO ISCHECKED
                Debug.WriteLine(HttpContext.Items.Keys);
                GenericIdentity gi = new GenericIdentity(user.Login);
                GenericPrincipal gp = new GenericPrincipal(gi,new string[] { "Admin"});
                Debug.WriteLine(gp.Identities);
                Debug.WriteLine(gp.Claims);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login");


        }
        //Logout method 

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }
    }
}
