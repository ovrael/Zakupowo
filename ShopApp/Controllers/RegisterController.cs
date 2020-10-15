using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopApp.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        // GET: Register/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Register/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Register/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                //Adding new User to database
                ViewData["FirstName"] = collection[1];
                ViewData["LastName"] = collection[2];
                ViewData["BirthDate"] = collection[3];
                ViewData["Login"] = collection[4];
                ViewData["EncryptedPassword"] = collection[5];
                ViewData["Email"] = collection[6];
                ViewData["Phone"] = collection[7];
                ViewData["Country"] = collection[8];
                ViewData["City"] = collection[9];

                var context = new ShopEntities();

                DateTime.TryParse((string)ViewData["BirthDate"], out DateTime birthDate);

                //Creating new user, UserID and CreationDate will be declared automatically
                var user = new Users()
                {
                    FirstName = (string)ViewData["FirstName"],
                    LastName = (string)ViewData["LastName"],
                    BirthDate = birthDate,
                    Login = (string)ViewData["Login"],
                    EncryptedPassword = (string)ViewData["EncryptedPassword"],
                    Email = (string)ViewData["Email"],
                    Phone = (string)ViewData["Phone"],
                    Country = (string)ViewData["Country"],
                    City = (string)ViewData["City"],
                };

                context.Users.Add(user);
                context.SaveChanges();


                return View("Index");
                //return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Register/Delete/5
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
    }
}
