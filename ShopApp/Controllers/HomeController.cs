using ShopApp.DAL;
using ShopApp.Models;
using ShopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopApp.Controllers
{
    public class HomeController : Controller
    {
        private ShopContext db = new ShopContext();
        public ActionResult Index()
        {
            HomeIndexView viewData;
            List<Category> categories;
            List<Offer> offers;


            categories = db.Categories.ToList();
            offers = db.Offers.ToList();

            viewData = new HomeIndexView
            {
                CategoryList = categories,
                OfferList = offers
            };

            return View(viewData);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}