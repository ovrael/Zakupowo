using System;
using System.Collections.Generic;
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

                ViewData["UserID"] = collection[1];
                ViewData["FirstName"] = collection[2];
                ViewData["LastName"] = collection[3];
                ViewData["BirthDate"] = collection[4];
                ViewData["Login"] = collection[5];
                ViewData["EncryptedPassword"] = collection[6];
                ViewData["Email"] = collection[7];
                ViewData["Phone"] = collection[8];
                ViewData["CreationDate"] = collection[9];
                ViewData["Country"] = collection[10];
                ViewData["City"] = collection[11];

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
