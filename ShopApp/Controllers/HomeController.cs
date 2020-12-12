using ShopApp.DAL;
using ShopApp.Models;
using ShopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using ShopApp.ViewModels;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

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
        [HttpGet]
        public ActionResult Kat(string type = "Offers", int KatID = 1, int page = 1)//We come here from
        {
            var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            OffersAndBundles offersAndBundles = new OffersAndBundles();
            if (type == "Offers")
            {
                var Offers = db.Offers.Where(i => i.Category.CategoryID == KatID && i.IsActive)
                    .OrderByDescending(i => i.CreationDate)
                    .Skip(20 * (page - 1))
                    .Take(20)
                    .ToList();
                if (Offers != null)
                {
                    offersAndBundles.Offers = Offers;
                    if (user != null)
                        offersAndBundles.FavouriteItemsIDs = user.FavouriteOffer
                            .Where(i => i.Offer.IsActive && Offers.Contains(i.Offer))
                            .Select(i => i.Offer.OfferID);
                }
                else
                    ViewBag.Message = "Brak ofert dla podanych filtrów";
            }
            if(type == "Bundles")
            {
            var Bundles = db.Bundles.Where(i => i.Offers.Where(x => x.Category.CategoryID == KatID).Any() && i.IsActive)
                    .OrderByDescending(i => i.CreationDate)
                    .Skip(20 * (page - 1))
                    .Take(20)
                    .ToList();
                if (Bundles != null)
                {
                    offersAndBundles.Bundles = Bundles;
                    if (user != null)
                        offersAndBundles.FavouriteItemsIDs = user.FavouriteOffer
                            .Where(i => i.Bundle.IsActive && Bundles.Contains(i.Bundle))
                            .Select(i => i.Bundle.BundleID);
                }
                else
                    ViewBag.Message = "Brak zestawów dla podanych filtrów";

            }
            return View(offersAndBundles);
        }
        //[HttpPost]
        //public ActionResult Kat(FormCollection collection)//We come here from
        //{

        //    if (type == "Offers")
        //        db.Offers.Where(i => i.Category.CategoryID == KatID).Skip(20 * (page - 1)).Take(20);
        //    if (type == "Bundles")
        //        db.Bundles.Where(i => i.Offers.Where(x => x.Category.CategoryID == KatID).Any()).Skip(20 * (page - 1)).Take(20);
        //    OffersAndBundles offersAndBundles = new OffersAndBundles()
        //    {

        //    };
        //    return RedirectToAction("Kat","Home",new { type = type, KatID = KatID, page = page});
        //}
        // OFFER VIEW

        #region FavouriteOfferManagement
        [HttpPost]
        public async Task<ActionResult> Fav(string type, int id)
        {
            List<string> errors = new List<string>(); // You might want to return an error if something wrong happened

            User User = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();

            if ((type == "Offer" || type == "Bundle") && User != null)
            {
                Favourite Fv = new Favourite();
                if (type == "Offer")
                {
                    Fv.Offer = db.Offers.Where(i => i.OfferID == id).FirstOrDefault();
                    var OfferList = User.FavouriteOffer.Where(i => i.Offer != null).ToList();
                    if(Fv.Offer != null && !OfferList.Where(i => i.Offer.OfferID == Fv.Offer.OfferID).Any())
                    {
                        Fv.User = User;
                        db.Favourites.Add(Fv);
                        Fv.Offer.FavouriteOffer.Add(Fv);
                        Fv.User.FavouriteOffer.Add(Fv);
                        ConcurencyHandling(db);
                    }
                }
                else
                {
                    Fv.Bundle = db.Bundles.Where(i => i.BundleID == id).FirstOrDefault();
                    var BundleList = User.FavouriteOffer.Where(i => i.Bundle != null).ToList();
                    if(Fv.Bundle != null && !BundleList.Where(i => i.Bundle.BundleID == Fv.Bundle.BundleID).Any())
                    {
                        Fv.User = User;
                        db.Favourites.Add(Fv);
                        Fv.Bundle.Favourites.Add(Fv);
                        Fv.User.FavouriteOffer.Add(Fv);
                        ConcurencyHandling(db);
                    }
                }
            }

            return Json(errors, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> UnFav(string type, int id) 
        {
            List<string> errors = new List<string>(); // You might want to return an error if something wrong happened

            User User = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();

            if ((type == "Offer" || type == "Bundle") && User != null)
            {
                if (type == "Offer")
                {
                    Favourite Fv = User.FavouriteOffer.Where(i => i.Offer != null && i.Offer.OfferID == id).FirstOrDefault();

                    if (Fv != null)
                    {
                        User.FavouriteOffer.Remove(Fv);
                        var offer = db.Offers.Where(i => i.OfferID == id).First();
                        offer.FavouriteOffer.Remove(Fv);
                        db.Favourites.Remove(Fv);
                        ConcurencyHandling(db);
                    }
                }
                else
                {
                    Favourite Fv = User.FavouriteOffer.Where(i => i.Bundle != null && i.Bundle.BundleID == id).FirstOrDefault();
                    if (Fv != null)
                    {
                        User.FavouriteOffer.Remove(Fv);
                        var offer = db.Bundles.Where(i => i.BundleID == id).First();
                        offer.Favourites.Remove(Fv);
                        db.Favourites.Remove(Fv);
                        ConcurencyHandling(db);
                    }
                }
            }

            return Json(errors, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [NonAction]
        public static void ConcurencyHandling(ShopContext db)
        {
            bool saved = false;
            while (!saved)
            {
                try
                {
                    db.SaveChanges();
                    saved = true;
                }
                catch (DbUpdateException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.PropertyNames)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];

                            proposedValues[property] = proposedValue;
                        }

                        // Refresh original values to bypass next concurrency check
                        entry.OriginalValues.SetValues(databaseValues);
                    }
                }
            }
        }
    }
}