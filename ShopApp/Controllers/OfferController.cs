using ShopApp.DAL;
using ShopApp.Models;
using ShopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopApp.ViewModels.User;
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
        [HttpPost]
        [Authorize]
        public ActionResult AddToBucket(string type, int? quantity, int? id)
        {
            List<string> errors = new List<string>();

            User user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
           
            if ((type == "Offer" || type == "Bundle") && user != null && quantity != null && id != null)
            {
                BucketItem bucketItem = new BucketItem();
                if (type == "Offer")
                {
                    bucketItem.Offer = db.Offers.Where(i => i.OfferID == id).FirstOrDefault();
                    if(user.Bucket.BucketItems.Any())
                    {
                        var OfferList = user.Bucket.BucketItems.Where(i => i.Offer != null).ToList();
                        if (bucketItem.Offer != null && !OfferList.Where(i => i.Offer.OfferID == bucketItem.Offer.OfferID).Any())
                        {
                            bucketItem.Quantity = (int)quantity;
                            bucketItem.TotalPrice = bucketItem.Quantity * bucketItem.Offer.Price;
                            db.BucketItems.Add(bucketItem);
                            user.Bucket.BucketItems.Add(bucketItem);
                            bucketItem.Bucket = user.Bucket;
                            ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                        }
                    }
                }
                else
                {
                    bucketItem.Bundle = db.Bundles.Where(i => i.BundleID == id).FirstOrDefault();
                    if (user.Bucket.BucketItems.Any())
                    {
                        var BundleList = user.Bucket.BucketItems.Where(i => i.Bundle != null).ToList();
                        if (bucketItem.Bundle != null && !BundleList.Where(i => i.Bundle.BundleID == bucketItem.Bundle.BundleID).Any())
                        {
                            bucketItem.Quantity = (int)quantity;
                            bucketItem.TotalPrice = bucketItem.Bundle.BundlePrice;
                            db.BucketItems.Add(bucketItem);
                            user.Bucket.BucketItems.Add(bucketItem);
                            bucketItem.Bucket = user.Bucket;
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