using ShopApp.DAL;
using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ShopApp.Controllers
{
    public class RegisterController : Controller
    {
        // private ShopContext db = new ShopContext();
        // GET: Register
        public ActionResult Usr()
        {
            return View();
        }

        // GET: Register/Create
        public ActionResult Create()
        {
            User user = new User();


            return View(user);
            
        }

        // POST: Register/Create
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (user.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(user.ImageUpload.FileName);
                string extension = Path.GetExtension(user.ImageUpload.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                user.ImagePath = "~/AppFiles/Images/" + fileName;
                user.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));


            }

            db.Users.Add(user);
            db.SaveChanges();
            ViewBag.Message = db.Users.ToList();
            return View();
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
        #endregion
    }
}
