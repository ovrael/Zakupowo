using ShopApp.DAL;
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
        



    }
}