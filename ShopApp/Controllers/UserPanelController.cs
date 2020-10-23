﻿using ShopApp.DAL;
using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopApp.Controllers
{
    public class UserPanelController : Controller
    {
        private ShopContext db = new ShopContext();
        // GET: UserPanel
        public ActionResult Index()
        {
            ViewBag.Message = db.Users.ToList();
            return View();
        }
        public ActionResult UserPanel()
        {
            ViewBag.Message = "Page to manage your profile.";

            return View();
        }
        public ActionResult Create(FormCollection collection)
        {
            try
            {

                User user = new User()
                {
                    FirstName = collection["FirstName"],
                    LastName = collection["LastName"],
                    Email = collection["Email"],

                };

                db.Users.Add(user);
                db.SaveChanges();


                return View("Success");

            }
            catch (Exception e) { return View(e.Message); }


        }
        public ActionResult AddOrEdit(int id=0)
        {
            User user = new User();
            return View(user);
        }
        [HttpPost]
        public ActionResult AddOrEdit()
        {
            return View();
        }


    }
}