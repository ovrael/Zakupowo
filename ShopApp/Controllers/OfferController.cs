using ShopApp.DAL;
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
using ShopApp.ViewModels;

namespace ShopApp.Controllers
{
    public class OfferController : Controller
    {
        private ShopContext db = new ShopContext();


        // GET: Offer
        //Displaying particular offer
        public ActionResult Index(int? OfferID)
        {
            var offer = db.Offers.Where(i => i.OfferID == OfferID && i.IsActive).FirstOrDefault();
            if (offer != null)
            {
                OfferIndexViewModel offerIndexViewModel = new OfferIndexViewModel
                {
                    Offer = offer
                };
                var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
                if (user != null)
                {
                    //Searching whether the offer is in User's Bucket or Favourite list
                    var InBucketOffer = user?.Bucket?.BucketItems?.Where(i => i.Offer != null && i.Offer.OfferID == OfferID).Select(i => i.Offer).FirstOrDefault();
                    offerIndexViewModel.IsInBucket = InBucketOffer != null;
                    var InFavouriteOffer = user?.FavouriteOffer?.Where(i => i.Offer != null && i.Offer.OfferID == OfferID).Select(i => i.Offer).FirstOrDefault();
                    offerIndexViewModel.IsInFavourite = InFavouriteOffer != null;
                }
                return View(offerIndexViewModel);
            }
            else
            {
                //Returning 404 when offer has been accessed not through site workflow (eg. changing URL to specific ID) 
                return new HttpStatusCodeResult(404);
            }
        }

        public ActionResult Bundle(int? BundleID)
        {
            if (BundleID != null || BundleID < 0)
            {
                var bundle = db.Bundles.Where(i => i.BundleID == BundleID).FirstOrDefault();
                var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
                if (user != null && bundle != null && bundle.Offers != null && bundle.Offers.Count() > 0)
                {
                    bool? isinbucket = user.Bucket.BucketItems?.Where(i => i.Offer == null).Select(i => i.Bundle).ToList().Contains(bundle);
                    bool? isinfavourite = user.FavouriteOffer?.Where(i => i.Offer == null).Select(i => i.Bundle).ToList().Contains(bundle);

                    List<OfferPicture> MainPictures = new List<OfferPicture>();
                    foreach (var offer in bundle.Offers)
                    {
                        MainPictures.Add(offer.OfferPictures.First());
                    }

                    BundleViewModel BundleViewModel = new BundleViewModel()
                    {
                        Bundle = bundle,
                        OffersList = bundle.Offers.ToList(),
                        IsInFavourite = isinfavourite ?? false,
                        IsInBucket = isinbucket ?? false,
                        MainPictures = MainPictures
                    };

                    return View(BundleViewModel);

                }
                else
                    return new HttpStatusCodeResult(500);
            }
            else
                return new HttpStatusCodeResult(404);
        }



        #region Favourites

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
                return new HttpStatusCodeResult(404);
            }
        }


        #region FavouriteOfferManagement
        [HttpPost]
        [Authorize]
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
                    if (Fv.Offer != null && !OfferList.Where(i => i.Offer.OfferID == Fv.Offer.OfferID).Any())
                    {
                        Fv.User = User;
                        db.Favourites.Add(Fv);
                        Fv.Offer.FavouriteOffer.Add(Fv);
                        Fv.User.FavouriteOffer.Add(Fv);
                        ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                    }
                }
                else
                {
                    Fv.Bundle = db.Bundles.Where(i => i.BundleID == id).FirstOrDefault();
                    var BundleList = User.FavouriteOffer.Where(i => i.Bundle != null).ToList();
                    if (Fv.Bundle != null && !BundleList.Where(i => i.Bundle.BundleID == Fv.Bundle.BundleID).Any())
                    {
                        Fv.User = User;
                        db.Favourites.Add(Fv);
                        Fv.Bundle.Favourites.Add(Fv);
                        Fv.User.FavouriteOffer.Add(Fv);
                        ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                    }
                }
            }

            return Json(errors, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
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
                        ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
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
                        ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                    }
                }
            }

            return Json(errors, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion


        #region Bucket

        //View of items that user has in bucket
        [HttpGet]
        [Authorize]
        public ActionResult Bucket()
        {
            var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                var BucketItems = user.Bucket.BucketItems.GroupBy(i => i.Offer != null ? i.Offer.User : i.Bundle.User);
                //Consider using Critical error page for below
                if (TempData["ErrorMessage"] == "TransactionRequestError")
                {
                    ViewBag.NotEveryBucketCouldHaveBeenSold = "Niestety, nie udało się zakupić wszystkich przedmiotów.";
                    ViewBag.PleaseContactWithSupport = "Po więcej informacji proszę skontaktować się z pomocą Zakupowo lub spróbować ponownie później.";
                }
                if (user.ShippingAdresses.Count() == 0)
                    ViewBag.UserHasNoShippingAddress = "Przed przejściem do kasy wymagane jest ustawienie adresu dostawy";
                if (user?.Bucket?.BucketItems == null)
                    ViewBag.BucketItemsCountZero = "Żeby przejść do kasy musisz mieć jakieś przedmioty w swoim koszyku.";
                if ((bool)!user?.IsActivated)
                    ViewBag.UserIsNotActivated = "Aby dokonać zakupu konto musi być aktywowane";
                return View(BucketItems);
            }
            else
                //Returning 404 when somehow user is authorized but not in Database
                return new HttpStatusCodeResult(404);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Bucket(FormCollection collection)
        {
            var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                if (user.ShippingAdresses.Count() == 0 || user?.Bucket?.BucketItems == null || !user.IsActivated)
                    return RedirectToAction("Bucket");
                if (collection != null)
                {
                    var bucketItems = user?.Bucket?.BucketItems?.ToList();
                    if (bucketItems != null)
                    {
                        var ActiveOffersInBucket = bucketItems.Where(i => i.Offer != null && i.Offer.IsActive).ToList();
                        var ActiveBundlesInBucket = bucketItems.Where(i => i.Bundle != null && i.Bundle.IsActive).ToList();
                        var CollectionSellers = collection.AllKeys.Where(i => i.StartsWith("SelectedSeller_")).Select(i => i.Substring(i.LastIndexOf("_") + 1).ToList());

                        List<string> SellersLogins = new List<string>();

                        foreach (var item in CollectionSellers)
                            SellersLogins.Add(new string(item.ToArray()));

                        List<BucketItem> BucketItemsListThatIsGoingToBeBought = new List<BucketItem>();
                        if (SellersLogins != null)
                        {
                            foreach (var item in collection.AllKeys)
                            {
                                if (item.StartsWith("Offer_quantity_") && int.TryParse(item.Substring(item.LastIndexOf("_") + 1), out int BucketItemOfferID))
                                {
                                    var BucketItemThatIsGoingToBeBought = ActiveOffersInBucket?.Where(i => i.BucketItemID == BucketItemOfferID).FirstOrDefault();
                                    if (BucketItemThatIsGoingToBeBought != null && SellersLogins != null && SellersLogins.Contains(BucketItemThatIsGoingToBeBought.Offer.User.Login) && int.TryParse(collection["Offer_quantity_" + BucketItemOfferID], out int OfferQuantity))
                                    {
                                        if (BucketItemThatIsGoingToBeBought.Offer.InStockNow >= OfferQuantity)
                                        {
                                            BucketItemThatIsGoingToBeBought.Quantity = OfferQuantity;
                                            BucketItemsListThatIsGoingToBeBought.Add(BucketItemThatIsGoingToBeBought);
                                        }
                                        else
                                        {
                                            ViewBag.ExceedingMaxAmount = "Nie wszystkie produkty mogą zostać zakupione w podanych ilościach";
                                            return View("CriticalWarningPage");
                                        }
                                    }
                                }
                                else if (item.StartsWith("Bundle_quantity_") && int.TryParse(item.Substring(item.LastIndexOf("_") + 1), out int BucketItemBundleID))
                                {
                                    var BucketItemThatIsGoingToBeBought = ActiveBundlesInBucket?.Where(i => i.BucketItemID == BucketItemBundleID).FirstOrDefault();
                                    if (BucketItemThatIsGoingToBeBought != null && SellersLogins != null && SellersLogins.Contains(BucketItemThatIsGoingToBeBought.Bundle.User.Login))
                                    {
                                        BucketItemsListThatIsGoingToBeBought.Add(BucketItemThatIsGoingToBeBought);
                                    }
                                }
                            }
                        }

                        //If collection offers bundles and sellers all were valid => View if not => Redirect
                        if (BucketItemsListThatIsGoingToBeBought.Count() > 0)
                        {
                            //Not sure what happens if we delete the bucketitems in between operations
                            foreach (var item in user.Order.BucketItems)
                                item.Order.Remove(user.Order);
                            user.Order.BucketItems.Clear();
                            db.SaveChanges();
                            user.Order.BucketItems = BucketItemsListThatIsGoingToBeBought;
                            db.SaveChanges();
                            foreach (var item in user.Order.BucketItems)
                                item.Order.Add(user.Order);

                            TempData["RedirectedFrom"] = "Bucket";
                            return RedirectToAction("Order");
                        }
                        return RedirectToAction("Bucket");
                    }
                }
            }
            return new HttpStatusCodeResult(404);
        }

        #region BucketManagement

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddToBucket(string type, int? id, int quantity = 1)
        {
            List<string> errors = new List<string>();

            User user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();

            if ((type == "Offer" || type == "Bundle") && user != null && id != null && user.Offers.Where(i => i.OfferID == id).FirstOrDefault() == null)
            {
                BucketItem NewBucketItem = new BucketItem();
                if (type == "Offer")
                {
                    NewBucketItem.Offer = db.Offers.Where(i => i.OfferID == id && i.IsActive).FirstOrDefault();
                    if (NewBucketItem.Offer != null)//We chceck if user called for existing and active offer
                    {
                        var OffersThatAreAlreadyInUsersBucket = user.Bucket?.BucketItems?.Where(i => i.Offer != null).ToList();
                        if (OffersThatAreAlreadyInUsersBucket != null && !OffersThatAreAlreadyInUsersBucket.Where(i => i.Offer.OfferID == NewBucketItem.Offer.OfferID).Any())//We check if user has already that offer in bucket.
                        {
                            NewBucketItem.Quantity = NewBucketItem.Offer.InStockNow >= (int)quantity ? (int)quantity : 1;
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

            return Json(errors, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> RemoveFromBucket(string type, int? id)
        {
            List<string> errors = new List<string>(); // You might want to return an error if something wrong happened

            User User = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();

            if ((type == "Offer" || type == "Bundle") && User != null)
            {
                if (type == "Offer")
                {
                    BucketItem UsersBucketItemThatIsMeantToBeRemoved = User?.Bucket?.BucketItems?.Where(i => i.Offer != null && i.Offer.OfferID == id).FirstOrDefault();

                    if (UsersBucketItemThatIsMeantToBeRemoved != null)
                    {
                        User.Bucket.BucketItems.Remove(UsersBucketItemThatIsMeantToBeRemoved);
                        Offer BucketItemsOFfer = UsersBucketItemThatIsMeantToBeRemoved.Offer;
                        BucketItemsOFfer.BucketItems.Remove(UsersBucketItemThatIsMeantToBeRemoved);
                        db.BucketItems.Remove(UsersBucketItemThatIsMeantToBeRemoved);
                        ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                    }
                }
                else
                {
                    BucketItem UsersBucketItemThatIsMeantToBeRemoved = User?.Bucket?.BucketItems?.Where(i => i.Bundle != null && i.Bundle.BundleID == id).FirstOrDefault();
                    if (UsersBucketItemThatIsMeantToBeRemoved != null)
                    {
                        User.Bucket.BucketItems.Remove(UsersBucketItemThatIsMeantToBeRemoved);
                        Bundle BucketItemsBundle = UsersBucketItemThatIsMeantToBeRemoved.Bundle;
                        BucketItemsBundle.BucketItems.Remove(UsersBucketItemThatIsMeantToBeRemoved);
                        db.BucketItems.Remove(UsersBucketItemThatIsMeantToBeRemoved);
                        ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                    }
                }
            }

            return Json(errors, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion


        #region Orders
        [HttpGet]
        [Authorize]
        public ActionResult Order()
        {
            if (TempData["RedirectedFrom"] != null && TempData["RedirectedFrom"] == "Bucket")
            {
                var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();

                if (user != null && user.Order.BucketItems != null && user.ShippingAdresses.Count() != 0)
                {
                    CashOutViewModel cashOutViewModel = new CashOutViewModel
                    {
                        GroupedBucketItems = user.Order.BucketItems.GroupBy(i => i.Offer != null ? i.Offer.User : i.Bundle.User).ToList(),
                        ShippingAdresses = user.ShippingAdresses
                    };
                    return View(cashOutViewModel);
                }
                else
                    //Returning 404 becuase managing the code to result in here has to be client-side code changes
                    return new HttpStatusCodeResult(404);
            }
            else
                return new HttpStatusCodeResult(404);

        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Order(CashOutViewModel cashOutViewModel, FormCollection collection)
        {
            if (collection["address-input"] != null && int.TryParse(collection["address-input"], out int result))
            {
                var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name && i.IsActivated).FirstOrDefault();
                //Odpowiednie errorr message dlaczego
                if (user != null)
                {
                    var order = user.Order?.BucketItems?.ToList();
                    var Address = user.ShippingAdresses?.Where(i => i.AdressID == result).FirstOrDefault();
                    List<BucketItem> ItemsThatCouldntBeenSold = new List<BucketItem>();
                    if (order != null && Address != null)
                    {
                        var grouped = order.GroupBy(i => i.Offer != null ? i.Offer.User : i.Bundle.User);
                        foreach (var seller in grouped)
                        {
                            var message = "Jestem zainteresowany zakupem wystawionego produktu proszę o odpowiedź.";
                            if (collection[$"message-input-{seller.Key.UserID}"] != null)
                                message = collection[$"message-input-{seller.Key.UserID}"];
                            if (!EmailManager.SendEmail(EmailManager.EmailType.TransactionRequest, seller.Key.FirstName, seller.Key.LastName, seller.Key.Email, user.Login, user.FirstName, user.LastName, seller.ToList(), message, Address))
                                //check that
                                seller.ToList().ForEach(i => ItemsThatCouldntBeenSold.Add(i));
                            else
                            {
                                Transaction transaction = new Transaction()
                                {
                                    Buyer = user,
                                    BucketItems = seller.ToList(),
                                    Seller = seller.Key,
                                    IsAccepted = false,
                                    IsChosen = false
                                };
                                db.Transactions.Add(transaction);
                                ConcurencyHandling.SaveChangesWithConcurencyHandling(db);

                                foreach (var offer in seller.ToList())
                                {
                                    offer.Transaction.Add(transaction);
                                }
                                ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                            }
                        }
                        if (ItemsThatCouldntBeenSold != null && ItemsThatCouldntBeenSold.Count() != 0)
                        {
                            TempData["ErrorMessage"] = "TransactionRequestError";
                        }
                        var BucketItemsInOrderTab = user.Order.BucketItems.ToArray();
                        for (int x = 0; x < BucketItemsInOrderTab.Count(); x++)
                        {
                            var item = BucketItemsInOrderTab.ElementAt(x);
                            if (ItemsThatCouldntBeenSold != null && ItemsThatCouldntBeenSold.Count() != 0 && ItemsThatCouldntBeenSold.Contains(item))
                                continue;
                            else
                            {
                                await RemoveFromBucket(item.Offer != null ? "Offer" : "Bundle", item.Offer != null ? item.Offer.OfferID : item.Bundle.BundleID);
                            }
                        }


                        user.Order.BucketItems.Clear();
                        ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                    }
                }
                //usunac ischosen z bazy
                //tworzymy historie transakcji
                //do bazy dodac w transakcji isconfirmed ischosen
                //Dodawanie i usuwanie z bucketa musza miec walidacje na transakcje
            }
            return RedirectToAction("Bucket");
        }

        #endregion




    }
}