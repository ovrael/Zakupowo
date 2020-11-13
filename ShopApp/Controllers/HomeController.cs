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
    [AllowAnonymous]
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

        public ActionResult Kat(int KatID)//We come here from index 
        {
            


            return View();
        }
        
        public ActionResult Offer(int OfferID)//We come here from index 
        {
            if (OfferID > 0)
            {
                Offer oferta = DataBase.SearchForOffer(OfferID);
                return View(oferta);
            }
            else
                return RedirectToAction("Index");
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