using ShopApp.DAL;
using ShopApp.Models;
using ShopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopApp.ViewModels.User;

namespace ShopApp.Controllers
{
    public class OfferController : Controller
    {
        private ShopContext db = new ShopContext();
        // GET: Offer
        public ActionResult Index(int? OfferID)
        {

            var offer = db.Offers.Where(i => i.OfferID == OfferID).FirstOrDefault();

            if (offer != null)
            {
                Offer viewOffer = DataBase.SearchForOffer((int)OfferID);
                return View(viewOffer);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


            //TODO MESSAGE WHY IT THREW ME AWAY FROM OFFER
            //return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Authorize]
        public ActionResult Index(FormCollection collection)
        {
            if (int.TryParse(collection["prodId"], out int result))
            {
                var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).Single();
                var offer = db.Offers.Where(i => i.OfferID == result).Single();
                var BucketItem = new BucketItem
                {
                    Quantity = Convert.ToInt32(collection["quantity"])
                };
                BucketItem.TotalPrice = offer.Price * BucketItem.Quantity;
                if (collection["choice"] == "DODAJ DO KOSZYKA")
                {
                    if (user.Bucket.BucketItems.Where(i => i.Offer.OfferID == offer.OfferID).Any())
                    {
                        ViewBag.BucketItem = "Już masz tę ofrtę w koszyku!";
                        return View(offer);
                    }
                    else
                    {
                        db.BucketItems.Add(BucketItem);
                        db.SaveChanges();
                        offer.BucketItems.Add(BucketItem);
                        db.SaveChanges();
                        user.Bucket.BucketItems.Add(BucketItem);
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Index", "Offer", new { OfferID = collection["prodId"] });
        }

        [Authorize]
        public ActionResult Bucket()
        {
            var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();
            var BucketItems = user.Bucket.BucketItems.GroupBy(i => i.Offer.User);
            return View(BucketItems);
        }
        //[HttpPost]
        //[Authorize]
        //public ActionResult Bucket(FormCollection collection)
        //{
        //    var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();
        //    var BucketItems = user.Bucket.BucketItems.GroupBy(i => i.Offer.User);
        //    //Dodawania transakcji dla każdego bucketItema którego sellerem jest collection["SelectedSeller_]""
        //    //foreach(var Seller in BucketItems)
        //    //    if(collection["SelectedSeller_"+ Seller.Key.Login])

        //    return View("SuccesfulShopping");
        //}
        [HttpGet]
        public ActionResult Favourites()
        {
            var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).SingleOrDefault();
            if (user != null)
            {
                List<Offer> favUserOffers = new List<Offer>();
                List<Bundle> favUserBundles = new List<Bundle>();

                foreach (var favOffer in user.FavouriteOffer)
                {
                    if (favOffer.Offer != null)
                        favUserOffers.Add(favOffer.Offer);
                    if (favOffer.Bundle != null)
                        favUserBundles.Add(favOffer.Bundle);
                }

                //var favouriteListItems = db.Favourites.Where(i => i.User.Login == user.Login && i.Offer.IsActive).Select(i => i.Offer);

                OffersAndBundles List = new OffersAndBundles
                {
                    Offers = favUserOffers,
                    Bundles = favUserBundles
                };
                //Dont forget about bundle logic

                return View(List);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}