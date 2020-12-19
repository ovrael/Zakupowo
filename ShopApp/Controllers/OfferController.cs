using ShopApp.DAL;
using ShopApp.Models;
using ShopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopApp.Utility;

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
                OfferIndexViewModel offerIndexViewModel = new OfferIndexViewModel();
                offerIndexViewModel.Offer = offer;
                var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
                if(user != null)
                { 
                    var OffersInBucket = user?.Bucket?.BucketItems?.Where(i => i.Offer != null).Select(i => i.Offer);
                    if (OffersInBucket != null)
                        offerIndexViewModel.IsInBucket = OffersInBucket.Where(i => i.OfferID == OfferID).Any();
                    var OffersInFavourite = user?.FavouriteOffer.Where(i => i.Offer != null).Select(i => i.Offer);
                    if (OffersInFavourite != null)
                        offerIndexViewModel.IsInFavourite = OffersInFavourite.Where(i => i.OfferID == OfferID).Any();
                }
                return View(offerIndexViewModel);
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }


            //TODO MESSAGE WHY IT THREW ME AWAY FROM OFFER
            //return RedirectToAction("Index", "Home");
        }
        //[HttpPost]
        //[Authorize]
        //public ActionResult Index(FormCollection collection)
        //{
        //    if (int.TryParse(collection["prodId"], out int result))
        //    {
        //        var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).Single();
        //        var offer = db.Offers.Where(i => i.OfferID == result).Single();
        //        var BucketItem = new BucketItem
        //        {
        //            Quantity = Convert.ToInt32(collection["quantity"])
        //        };
        //        BucketItem.TotalPrice = offer.Price * BucketItem.Quantity;
        //        if (collection["choice"] == "DODAJ DO KOSZYKA")
        //        {
        //            if (user.Bucket.BucketItems.Where(i => i.Offer.OfferID == offer.OfferID).Any())
        //            {
        //                ViewBag.BucketItem = "Już masz tę ofrtę w koszyku!";
        //                return View(offer);
        //            }
        //            else
        //            {
        //                db.BucketItems.Add(BucketItem);
        //                db.SaveChanges();
        //                offer.BucketItems.Add(BucketItem);
        //                db.SaveChanges();
        //                user.Bucket.BucketItems.Add(BucketItem);
        //                db.SaveChanges();
        //            }
        //        }
        //    }
        //    return RedirectToAction("Index", "Offer", new { OfferID = collection["prodId"] });
        //}
        [HttpPost]
        [Authorize]
        public ActionResult AddToBucket(string type, int? id, int quantity = 1)
        {
            List<string> errors = new List<string>();

            User user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
           
            if ((type == "Offer" || type == "Bundle") && user != null && id != null)
            {
                BucketItem NewBucketItem = new BucketItem();
                if (type == "Offer")
                {
                    NewBucketItem.Offer = db.Offers.Where(i => i.OfferID == id && i.IsActive).FirstOrDefault();
                    if(NewBucketItem.Offer != null)//We chceck if user called for existing and active offer
                    {
                        var OffersThatAreAlreadyInUsersBucket = user.Bucket?.BucketItems?.Where(i => i.Offer != null).ToList();
                        if (OffersThatAreAlreadyInUsersBucket != null && !OffersThatAreAlreadyInUsersBucket.Where(i => i.Offer.OfferID == NewBucketItem.Offer.OfferID).Any())//We check if user has already that offer in bucket.
                        {
                            NewBucketItem.Quantity = NewBucketItem.Offer.InStockNow <= (int)quantity? (int)quantity : 1;
                            NewBucketItem.TotalPrice = NewBucketItem.Quantity * NewBucketItem.Offer.Price;
                            db.BucketItems.Add(NewBucketItem);
                            user.Bucket.BucketItems.Add(NewBucketItem);
                            NewBucketItem.Bucket = user.Bucket;
                            ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                        }
                    }
                }
                else
                {
                    NewBucketItem.Bundle = db.Bundles.Where(i => i.BundleID == id && i.IsActive).FirstOrDefault();
                    if (NewBucketItem.Bundle != null)//We chceck if user called for existing and active bundle
                    {
                        var BundlesThatAreAlreadyInUsersBucket = user.Bucket.BucketItems.Where(i => i.Bundle != null).ToList();
                        if (NewBucketItem.Bundle != null && !BundlesThatAreAlreadyInUsersBucket.Where(i => i.Bundle.BundleID == NewBucketItem.Bundle.BundleID).Any())//We check if user has already that bundle in bucket.
                        {
                            NewBucketItem.Quantity = 1;
                            NewBucketItem.TotalPrice = NewBucketItem.Bundle.BundlePrice;
                            db.BucketItems.Add(NewBucketItem);
                            user.Bucket.BucketItems.Add(NewBucketItem);
                            NewBucketItem.Bucket = user.Bucket;
                            ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                        }
                    }
                }
            }

            return Json(errors,JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult Bucket()
        {
            var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                var BucketItems = user.Bucket.BucketItems.GroupBy(i => i.Offer.User);
                return View(BucketItems); 
            }
            else
                return new HttpStatusCodeResult(404);
        }
        
        [HttpPost]
        [Authorize]
        public ActionResult Bucket(FormCollection collection)
        {
            var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                var BucketItems = user.Bucket.BucketItems.GroupBy(i => i.Offer.User);
                //Dodawania transakcji dla każdego bucketItema którego sellerem jest collection["SelectedSeller_]""
                //foreach(var Seller in BucketItems)
                //    if(collection["SelectedSeller_"+ Seller.Key.Login])

                return View("SuccesfulShopping");
            }
            else
                return new HttpStatusCodeResult(404);
        }
        [HttpGet]
        [Authorize]
        public ActionResult Favourites()
        {
            var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).SingleOrDefault();
            if (user != null)
            {
                List<Offer> favUserOffers = new List<Offer>();
                List<Bundle> favUserBundles = new List<Bundle>();

                foreach (var favOffer in user.FavouriteOffer)
                {
                    if (favOffer != null && favOffer.Offer != null)
                    {
                        favUserOffers.Add(favOffer.Offer);
                        continue;
                    }
                    else if (favOffer != null && favOffer.Bundle != null)
                    {
                        favUserBundles.Add(favOffer.Bundle);
                    }
                }

                OffersAndBundles List = new OffersAndBundles
                {
                    Offers = favUserOffers,
                    Bundles = favUserBundles
                };

                return View(List);
            }
            else
            {
                return  new HttpStatusCodeResult(404);
            } 
        }
    }
}