﻿using ShopApp.DAL;
using ShopApp.Models;
using ShopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopApp.Utility;
using System.Threading.Tasks;
using System.Diagnostics;

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
                OfferIndexViewModel offerIndexViewModel = new OfferIndexViewModel
                {
                    Offer = offer
                };
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
                            NewBucketItem.Quantity = NewBucketItem.Offer.InStockNow >= (int)quantity? (int)quantity : 1;
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
            //IdOferty
            //Ilosc
            //Suma
            //
            var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if(user != null)
            {
                if(collection != null)
                {
                    var bucketItems = user?.Bucket?.BucketItems?.ToList();
                    if(bucketItems != null)
                    {
                        var OffersIDsInBucket = user.Bucket.BucketItems.Where(i => i.Offer != null && i.Offer.IsActive).ToList();
                        var BundlesIDsInBucket = user.Bucket.BucketItems.Where(i => i.Bundle != null && i.Bundle.IsActive).ToList();
                        var CollectionSellers = collection.AllKeys.Where(i => i.StartsWith("SelectedSeller_")).Select(i => i.Substring(i.LastIndexOf("_") + 1).ToList());

                        List<string> SellersLogins = new List<string>();

                        foreach (var item in CollectionSellers)
                            SellersLogins.Add( new string (item.ToArray()));

                        List<BucketItem> BucketItemsListThatIsGoingToBeBought = new List<BucketItem>();

                        foreach(var item in collection.AllKeys)
                        {
                            if(item.StartsWith("Offer_quantity_") && int.TryParse(item.Substring(item.LastIndexOf("_") + 1),out int BucketItemOfferID))
                            {
                                var BucketItemThatIsGoingToBeBought = OffersIDsInBucket.Where(i => i.BucketItemID == BucketItemOfferID).FirstOrDefault();
                                if (BucketItemThatIsGoingToBeBought != null && SellersLogins.Contains(BucketItemThatIsGoingToBeBought.Offer.User.Login) &&int.TryParse(collection["Offer_quantity_" + BucketItemOfferID],out int OfferQuantity))
                                {
                                    if (BucketItemThatIsGoingToBeBought.Offer.InStockNow >= OfferQuantity)
                                        BucketItemsListThatIsGoingToBeBought.Add(BucketItemThatIsGoingToBeBought);
                                    else
                                        ViewBag.ExceedingMaxAmount = "Nie wszystkie produkty mogą zostać zakupione w podanych ilościach";
                                }
                            }
                            else if (item.StartsWith("Bundle_quantity_") && int.TryParse(item.Substring(item.LastIndexOf("_") + 1), out int BucketItemBundleID))
                            {
                                var BucketItemThatIsGoingToBeBought = BundlesIDsInBucket.Where(i => i.BucketItemID == BucketItemBundleID).FirstOrDefault();
                                if (BucketItemThatIsGoingToBeBought != null && SellersLogins.Contains(BucketItemThatIsGoingToBeBought.Bundle.User.Login))
                                {
                                    BucketItemsListThatIsGoingToBeBought.Add(BucketItemThatIsGoingToBeBought);
                                }
                            }
                        }
                        //Tworzenie tabeli dla Zamówienia
                        return RedirectToAction("DoKasy");
                    }
                }
            }
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


                List<int> OffersIDsInBucket = new List<int>();
                List<int> BundlesIDsInBucket = new List<int>();


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

                OffersIDsInBucket = user?.Bucket?.BucketItems?.Where(i => i.Offer != null && i.Offer.IsActive).Select(i => i.Offer.OfferID).ToList();

                BundlesIDsInBucket = user?.Bucket?.BucketItems?.Where(i => i.Bundle != null && i.Bundle.IsActive).Select(i => i.Bundle.BundleID).ToList();

                OffersAndBundles list = new OffersAndBundles
                {
                    Offers = favUserOffers,
                    Bundles = favUserBundles,
                    InBucketOffersIDs = OffersIDsInBucket,
                    InBucketBundlesIDs = BundlesIDsInBucket
                };

                return View(list);
            }
            else
            {
                return  new HttpStatusCodeResult(404);
            } 
        }
    }
}