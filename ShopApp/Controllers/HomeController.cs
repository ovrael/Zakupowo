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
            return View();
        }
        public ActionResult Kat(int KatID = 1)//We come here from
        {
            return View(db.Offers.Where(i => i.Category.CategoryID == KatID && i.IsActive));
        }
        // OFFER VIEW

        #region FavouriteOfferManagement
        [HttpPost]
        public ActionResult Fav(string type, int id)//We come here from index 
        {
            List<string> errors = new List<string>(); // You might want to return an error if something wrong happened

            User User = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if(type == "Offer" || type =="Bundle")
            {
                Favourite Fv = new Favourite();
            if (type == "Offer")
            {
                Fv.Offer = db.Offers.Where(i => i.OfferID == id).First();
                Fv.User = User;
                    db.Favourites.Add(Fv);
                    db.SaveChanges();

                    var offer = db.Offers.Where(i => i.OfferID == id).First();
                    offer.FavouriteOffer.Add(Fv);
                    db.SaveChanges();

                    User.FavouriteOffer.Add(Fv);
                    db.SaveChanges();
                }
            else
            { 
                Fv.Bundle = db.Bundles.Where(i => i.BundleID == id).First();
                Fv.User = User;
                    db.Favourites.Add(Fv);
                    db.SaveChanges();

                    var offer = db.Bundles.Where(i => i.BundleID == id).First();
                    offer.Favourites.Add(Fv);
                    db.SaveChanges();

                    User.FavouriteOffer.Add(Fv);
                    db.SaveChanges();
                }
            }

            return Json(errors, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UnFav(string type, int id)//We come here from index 
        {

            List<string> errors = new List<string>(); // You might want to return an error if something wrong happened

            User User = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (type == "Offer" || type == "Bundle")
            {
                Favourite Fv = db.Favourites.Where(i => (i.Offer.OfferID == id || i.Bundle.BundleID == id) && i.User.UserID == User.UserID).FirstOrDefault();
                if (type == "Offer")
                {
                    if(Fv.Offer != null)
                    {
                        User.FavouriteOffer.Remove(Fv);
                        db.SaveChanges();

                        var offer = db.Offers.Where(i => i.OfferID == id).First();
                        offer.FavouriteOffer.Remove(Fv);
                        db.SaveChanges();

                        db.Favourites.Remove(Fv);
                        db.SaveChanges();
                    }
                }
                else
                {
                    if (Fv.Bundle != null)
                    {
                        User.FavouriteOffer.Remove(Fv);
                        db.SaveChanges();

                        var offer = db.Bundles.Where(i => i.BundleID == id).First();
                        offer.Favourites.Remove(Fv);
                        db.SaveChanges();

                        db.Favourites.Remove(Fv);
                        db.SaveChanges();
                    }
                }
            }

            return Json(errors, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}