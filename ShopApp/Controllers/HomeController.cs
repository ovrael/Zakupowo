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
        // CATEGORIES VIEW
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Kat(int KatID = 1)
        {
            return View(db.Offers.Where(i => i.Category.CategoryID == KatID));
        }
        // OFFER VIEW
        public ActionResult Offer(int? OfferID)
        {
            //TODO MESSAGE WHY IT THREW ME AWAY FROM OFFER
            if (db.Offers.Where(i => i.OfferID == OfferID).FirstOrDefault().IsActive)
            { 
                Offer oferta = DataBase.SearchForOffer((int)OfferID);
                return View(oferta);
            }
            return RedirectToAction("Index");
        }

        #region FavouriteOfferManagement
        [HttpPost]
        public ActionResult Fav(int id)//We come here from index 
        {
            List<string> errors = new List<string>(); // You might want to return an error if something wrong happened

            User User = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();

            FavouriteOffer FvOff = new FavouriteOffer
            {
                Offer = db.Offers.Where(i => i.OfferID == id).First(),
                User = User
            };

            db.FavouriteOffers.Add(FvOff);
            db.SaveChanges();

            var offer = db.Offers.Where(i => i.OfferID == id).First();
            offer.FavouriteOffer.Add(FvOff);
            db.SaveChanges();

            User.FavouriteOffer.Add(FvOff);
            db.SaveChanges();

            return Json(errors, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UnFav(int id)//We come here from index 
        {
            User User = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();

            List<string> errors = new List<string>(); // You might want to return an error if something wrong happened
            var FvOff = db.FavouriteOffers.Where(i => i.Offer.OfferID == id).First();
            if (User.FavouriteOffer.Contains(FvOff))
            {
                User.FavouriteOffer.Remove(FvOff);
                db.SaveChanges();

                var offer = db.Offers.Where(i => i.OfferID == id).First();
                offer.FavouriteOffer.Remove(FvOff);
                db.SaveChanges();

                db.FavouriteOffers.Remove(FvOff);
                db.SaveChanges();
            }

            return Json(errors, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}