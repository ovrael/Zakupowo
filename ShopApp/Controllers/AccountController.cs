using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopApp.DAL;
using ShopApp.Models;

namespace ShopApp.Controllers
{
    public class AccountController : Controller
    {
        private ShopContext databaseContext = new ShopContext();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        // GET: Account/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            string userName = collection[0];
            string userLastName = collection[1];
            string userLogin = collection[2];
            string userEmail = collection[3];
            string userEncryptedPassword = collection[4];

            User addedUser = new User()
            {
                FirstName = userName,
                LastName = userLastName,
                Login = userLogin,
                Email = userEmail,
                EncryptedPassword = userEncryptedPassword,
                BirthDate = DateTime.Now,
                CreationDate = DateTime.Now
            };

            if (ModelState.IsValid)
            {
                databaseContext.Users.Add(addedUser);
                databaseContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("UserCreate");
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Account/Edit/5
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

        // GET: Account/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Account/Delete/5
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

        // KOD KAMILA - USELESS??
        //   //POST: Register/Create
        //  [HttpPost]
        //  public ActionResult Create(User user)
        //  {
        //      if (user.ImageUpload != null)
        //      {
        //          string fileName = Path.GetFileNameWithoutExtension(user.ImageUpload.FileName);
        //          string extension = Path.GetExtension(user.ImageUpload.FileName);
        //          fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
        //          user.ImagePath = "~/AppFiles/Images/" + fileName;
        //          user.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));


        //      }

        //      db.Users.Add(user);
        //      db.SaveChanges();
        //      ViewBag.Message = db.Users.ToList();
        //      return View();
        //  }


    }
}
