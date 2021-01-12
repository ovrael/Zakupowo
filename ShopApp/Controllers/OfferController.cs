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

namespace ShopApp.Controllers
{
    public class OfferController : Controller
    {
        private ShopContext db = new ShopContext();


        // GET: Offer
        //Displaying particular offer
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

        [HttpGet]
        public ActionResult Bundle(int? BundleID)
        {
            if (BundleID != null || BundleID < 0)
            {
                var bundle = db.Bundles.Where(i => i.BundleID == BundleID).FirstOrDefault();
                if (bundle != null && bundle.Offers != null && bundle.Offers.Count() > 0)
                {
                    List<OfferPicture> MainPictures = new List<OfferPicture>();
                    foreach (var offer in bundle.Offers)
                    {
                        MainPictures.Add(offer.OfferPictures.First());
                    }
                    BundleViewModel BundleViewModel = new BundleViewModel()
                    {
                        Bundle = bundle,
                        OffersList = bundle.Offers.ToList(),
                        MainPictures = MainPictures
                    };

                    var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();

                    if (user != null)
                    {
                        bool? isinbucket = user.Bucket.BucketItems?.Where(i => i.Offer == null).Select(i => i.Bundle).ToList().Contains(bundle);
                        bool? isinfavourite = user.FavouriteOffer?.Where(i => i.Offer == null).Select(i => i.Bundle).ToList().Contains(bundle);

                        List<int> InFavouriteOffersIDs = new List<int>();
                        List<int> InBucketOffersIDs = new List<int>();

                        foreach (var favOffer in user.FavouriteOffer)
                        {
                            if (favOffer != null && favOffer.Offer != null)
                            {
                                InFavouriteOffersIDs.Add(favOffer.Offer.OfferID);
                            }
                        }

                        foreach (var BucketItem in user.Bucket.BucketItems)
                        {
                            if (BucketItem != null && BucketItem.Offer != null)
                            {
                                InBucketOffersIDs.Add(BucketItem.Offer.OfferID);
                            }
                        }

                        BundleViewModel.InFavouriteOffersIDs = InFavouriteOffersIDs;
                        BundleViewModel.InBucketOffersIDs = InBucketOffersIDs;
                        BundleViewModel.IsInFavourite = isinfavourite ?? false;
                        BundleViewModel.IsInBucket = isinbucket ?? false;
                    }


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
        public async Task<ActionResult> Fav(string type, int? id)
        {
            string error = string.Empty;

            User User = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();

            if ((type == "Offer" || type == "Bundle") && User != null && id != null)
            {
                Favourite Favourite = new Favourite();
                if (type == "Offer")
                {
                    var Offer = db.Offers.Where(i => i.OfferID == id && i.IsActive).FirstOrDefault();
                    if (Offer != null)
                    {
                        bool? AlreadyInFav = User.FavouriteOffer?.Where(i => i.Offer != null && i.Offer.OfferID == id).Any();
                        if (AlreadyInFav == false)
                        {
                            bool? IsOwner = User.Offers?.Where(i => i.OfferID == id).Any();
                            if (IsOwner == false)
                            {
                                Favourite.Offer = Offer;
                                Favourite.User = User;
                                db.Favourites.Add(Favourite);
                                Favourite.Offer.FavouriteOffer.Add(Favourite);
                                Favourite.User.FavouriteOffer.Add(Favourite);
                                ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                            }
                            else
                                error = "Ta oferta należy do Ciebie";
                        }
                        else
                            error = "Już posiadasz tę ofertę w ulubionych";
                    }
                    else
                        error = "Podana oferta nie istnieje lub jest nieaktywna";
                }
                else
                {
                    var Bundle = db.Bundles.Where(i => i.BundleID == id && i.IsActive).FirstOrDefault();
                    if (Bundle != null)
                    {
                        bool? AlreadyInFav = User.FavouriteOffer?.Where(i => i.Bundle != null && i.Bundle.BundleID == id && i.Bundle.IsActive).Any();
                        if (AlreadyInFav == false)
                        {
                            bool? IsOwner = User.Bundles?.Where(i => i.BundleID == id).Any();
                            if (IsOwner == false)
                            {
                                Favourite.Bundle = Bundle;
                                Favourite.User = User;
                                db.Favourites.Add(Favourite);
                                Favourite.Bundle.Favourites.Add(Favourite);
                                Favourite.User.FavouriteOffer.Add(Favourite);
                                ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                            }
                            else
                                error = "Ten zestaw należy do Ciebie";
                        }
                        else
                            error = "Już posiadasz ten zestaw w ulubionych";
                    }
                    else
                        error = "Podany zestaw nie istnieje lub jest nieaktywny";
                }
            }
            else
                error = "Wprowadzone dane są nieprawidłowe";

            return Json(error, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> UnFav(string type, int? id)
        {
            string error = string.Empty;

            User User = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();

            if ((type == "Offer" || type == "Bundle") && User != null && id != null && id > 0)
            {
                if (type == "Offer")
                {
                    var Offer = db.Offers.Where(i => i.OfferID == id).FirstOrDefault(); //Offer might not necessarily be active
                    if (Offer != null)
                    {
                        Favourite Favourite = User.FavouriteOffer?.Where(i => i.Offer != null && i.Offer.OfferID == id).FirstOrDefault();
                        if (Favourite != null)
                        {
                            //Not checking if hes an owner because he wouldn't been able to add to fav in the first place.
                            User.FavouriteOffer.Remove(Favourite);
                            Offer.FavouriteOffer.Remove(Favourite);
                            db.Favourites.Remove(Favourite);
                            ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                        }
                        else
                            error = "Nie posiadasz takiej oferty w ulubionych";
                    }
                    else
                        error = "Podana oferta nie istnieje";
                }
                else
                {
                    var Bundle = db.Bundles?.Where(i => i.BundleID == id).FirstOrDefault(); //Bundle might not necessarily be active
                    if (Bundle != null)
                    {
                        Favourite Favourite = User.FavouriteOffer.Where(i => i.Bundle != null && i.Bundle.BundleID == id).FirstOrDefault();
                        if (Favourite != null)
                        {
                            User.FavouriteOffer.Remove(Favourite);
                            Bundle.Favourites.Remove(Favourite);
                            db.Favourites.Remove(Favourite);
                            ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                        }
                        else
                            error = "Nie posiadasz takiego zestawu w ulubionych";
                    }
                    else
                        error = "Podany zestaw nie istnieje";
                }
            }
            else
                error = "Wprowadzone dane są nieprawidłowe";

            return Json(error, JsonRequestBehavior.AllowGet);
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
                //Consinder deleting 0 quant items;
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
            Stopwatch watcher = new Stopwatch();
            watcher.Start();
            var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                if (user.ShippingAdresses.Count() == 0 || user?.Bucket?.BucketItems == null || !user.IsActivated)
                    return RedirectToAction("Bucket");
                if (collection != null)
                {
                    var UsersBucketItems = user?.Bucket?.BucketItems?.ToList();
                    if (UsersBucketItems != null)
                    {
                        var InBucketOffers = UsersBucketItems.Where(i => i.Offer != null && i.Offer.IsActive).ToList();
                        var InBucketBundles = UsersBucketItems.Where(i => i.Bundle != null && i.Bundle.IsActive).ToList();
                        var SellersLogins = collection.AllKeys.Where(i => i.StartsWith("SelectedSeller_")).Select(i => i.Substring(i.LastIndexOf("_") + 1)).ToList();

                        List<BucketItem> BucketItemsListToBuy = new List<BucketItem>();
                        if (SellersLogins != null)
                        {
                            foreach (var item in collection.AllKeys)
                            {
                                if (item.StartsWith("Offer_quantity_") && int.TryParse(item.Substring(item.LastIndexOf("_") + 1), out int BucketItemOfferID))
                                {
                                    var SelectedBucketItemOffer = InBucketOffers?.Where(i => i.BucketItemID == BucketItemOfferID).FirstOrDefault();
                                    if (SelectedBucketItemOffer != null && SellersLogins.Contains(SelectedBucketItemOffer.Offer.User.Login) && int.TryParse(collection["Offer_quantity_" + BucketItemOfferID], out int BucketItemOfferQuantity))
                                    {
                                        if (SelectedBucketItemOffer.Offer.InStockNow >= BucketItemOfferQuantity)
                                        {
                                            SelectedBucketItemOffer.Quantity = BucketItemOfferQuantity;
                                            BucketItemsListToBuy.Add(SelectedBucketItemOffer);
                                        }

                                        //else
                                        //{
                                        //    ViewBag.ExceedingMaxAmount = "Nie wszystkie produkty mogą zostać zakupione w podanych ilościach";
                                        //    return View("CriticalErrorView");
                                        //}
                                        // -----
                                        //Check saved time
                                    }
                                }
                                else if (item.StartsWith("Bundle_quantity_") && int.TryParse(item.Substring(item.LastIndexOf("_") + 1), out int BucketItemBundleID))
                                {
                                    var SelecetedBucketItemBundle = InBucketBundles?.Where(i => i.BucketItemID == BucketItemBundleID).FirstOrDefault();
                                    if (SelecetedBucketItemBundle != null && SellersLogins != null && SellersLogins.Contains(SelecetedBucketItemBundle.Bundle.User.Login))
                                    {
                                        BucketItemsListToBuy.Add(SelecetedBucketItemBundle);
                                    }
                                }
                            }
                        }
                        Debug.WriteLine("Przed ifem " + watcher.ElapsedMilliseconds);

                        //If collection offers bundles and sellers all were valid => View if not => Redirect
                        if (BucketItemsListToBuy.Count() > 0)
                        {
                            try
                            {
                                ////Not sure what happens if we delete the bucketitems in between operations
                                foreach (var item in user.Order.BucketItems)
                                    if (item != null && item.Order.Contains(user.Order))
                                        item.Order.Remove(user.Order);

                                Debug.WriteLine("1 " + watcher.ElapsedMilliseconds);


                                //user.Order.BucketItems.Clear();
                                //ConcurencyHandling.SaveChangesWithConcurencyHandling(db);

                                user.Order.BucketItems = BucketItemsListToBuy;

                                Debug.WriteLine("2 " + watcher.ElapsedMilliseconds);


                                foreach (var item in user.Order.BucketItems)
                                    item.Order.Add(user.Order);

                                ConcurencyHandling.SaveChangesWithConcurencyHandling(db);


                                Debug.WriteLine("3 " + watcher.ElapsedMilliseconds);

                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine(e.StackTrace);
                                return new HttpStatusCodeResult(500);
                            }

                            TempData["RedirectedFrom"] = "Bucket";
                            Debug.WriteLine("Przed stopem " + watcher.ElapsedMilliseconds);
                            watcher.Stop();
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
        public async Task<ActionResult> AddToBucket(string type, int? id, string quantity)
        {
            string error = string.Empty;

            User user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();

            if ((type == "Offer" || type == "Bundle") && user != null && id != null && quantity != null)
            {
                BucketItem NewBucketItem = new BucketItem();
                if (type == "Offer")
                {
                    var Offer = db.Offers.Where(i => i.OfferID == id && i.IsActive).FirstOrDefault();
                    if (Offer != null)//We chceck if user called for existing and active offer
                    {
                        bool? AlreadyInBucket = user.Bucket?.BucketItems?.Where(i => i.Offer != null && i.Offer.OfferID == id).Any();
                        if (AlreadyInBucket == false)
                        {
                            bool? IsOwner = user.Offers?.Where(i => i.OfferID == id).Any();
                            if (IsOwner == false)
                            {
                                if (int.TryParse(quantity, out int QuantityAsInt))
                                {
                                    if (Offer.InStockNow < QuantityAsInt || QuantityAsInt < 1)
                                    {
                                        error = "Przekroczono dostępną ilość danego produktu";
                                    }
                                    else
                                    {
                                        NewBucketItem.Offer = Offer;
                                        NewBucketItem.Quantity = QuantityAsInt;
                                        NewBucketItem.TotalPrice = QuantityAsInt * Offer.Price;
                                        db.BucketItems.Add(NewBucketItem);

                                        user.Bucket.BucketItems.Add(NewBucketItem);
                                        NewBucketItem.Bucket = user.Bucket;
                                        Offer.BucketItems.Add(NewBucketItem);
                                        ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                                    }
                                }
                                else
                                    error = "Ilość musi być liczbą";
                            }
                            else
                                error = "Nie możesz dodać do koszyka swojej oferty";
                        }
                        else
                            error = "Już posiadasz tę ofertę";
                    }
                    else
                        error = "Nie znaleziono takiej oferty";
                }
                else
                {
                    var Bundle = db.Bundles.Where(i => i.BundleID == id && i.IsActive).FirstOrDefault();
                    if (Bundle != null)//We chceck if user called for existing and active Bundle
                    {
                        bool? AlreadyInBucket = user.Bucket?.BucketItems?.Where(i => i.Bundle != null && i.Bundle.BundleID == id).Any();
                        if (AlreadyInBucket == false)
                        {
                            bool? IsOwner = user.Bundles?.Where(i => i.BundleID == id).Any();
                            if (IsOwner == false)
                            {
                                NewBucketItem.Quantity = 1;
                                NewBucketItem.TotalPrice = Bundle.BundlePrice;
                                NewBucketItem.Bundle = Bundle;
                                db.BucketItems.Add(NewBucketItem);

                                user.Bucket.BucketItems.Add(NewBucketItem);
                                NewBucketItem.Bucket = user.Bucket;
                                Bundle.BucketItems.Add(NewBucketItem);
                                ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                            }
                            else
                                error = "Nie możesz dodać do koszyka swojego zestawu";
                        }
                        else
                            error = "Już posiadasz ten zestaw";
                    }
                    else
                        error = "Nie znaleziono takiego zesatwu";
                }
            }
            else
                error = "Wprowadzone dane są niepoprawne";
            return Json(error, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> RemoveFromBucket(string type, int? id)
        {
            string error = string.Empty;

            User User = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();

            if ((type == "Offer" || type == "Bundle") && User != null && id != null && id > 0)
            {
                if (type == "Offer")
                {
                    var Offer = db.Offers.Where(i => i.OfferID == id).FirstOrDefault();
                    if (Offer != null)
                    {
                        BucketItem BucketItemToRemove = User.Bucket?.BucketItems?.Where(i => i.Offer != null && i.Offer.OfferID == id).FirstOrDefault();
                        if (BucketItemToRemove != null)//We chceck if user called for existing and active offer
                        {
                            User.Bucket.BucketItems.Remove(BucketItemToRemove);
                            Offer.BucketItems.Remove(BucketItemToRemove);
                            db.BucketItems.Remove(BucketItemToRemove);
                            ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                        }
                        else
                            error = "Nie posiadasz tej oferty w swoim koszyku";
                    }
                    else
                        error = "Wybrana oferta nie istnieje";
                }
                else
                {
                    var Bundle = db.Bundles.Where(i => i.BundleID == id && i.IsActive).FirstOrDefault();
                    if (Bundle != null)
                    {
                        BucketItem BucketItemToRemove = User?.Bucket?.BucketItems?.Where(i => i.Bundle != null && i.Bundle.BundleID == id).FirstOrDefault();
                        if (BucketItemToRemove != null)
                        {
                            User.Bucket.BucketItems.Remove(BucketItemToRemove);
                            Bundle.BucketItems.Remove(BucketItemToRemove);
                            db.BucketItems.Remove(BucketItemToRemove);
                            ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                        }
                        else
                            error = "Nie posiadasz takiego zestawu w koszyku";
                    }
                    else
                        error = "Wybrany zestaw nie istnieje";
                }
            }
            else
                error = "Wprowadzone dane są niepoprawne";

            return Json(error, JsonRequestBehavior.AllowGet);
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
                            if (collection[$"message-input-{seller.Key.UserID}"] != null && collection[$"message-input-{seller.Key.UserID}"].Length != 0)
                                message = collection[$"message-input-{seller.Key.UserID}"];
                            if (!EmailManager.SendEmail(EmailManager.EmailType.TransactionRequest, seller.Key.FirstName, seller.Key.LastName, seller.Key.Email, user.Login, user.FirstName, user.LastName, seller.ToList(), message, Address))
                                //check that
                                seller.ToList().ForEach(i => ItemsThatCouldntBeenSold.Add(i));
                            else
                            {
                                Transaction transaction = new Transaction()
                                {
                                    Buyer = user,
                                    Seller = seller.Key,
                                    BucketItems = seller.ToList(), // NIE PRZYPISUJE ALE NIE WYWALA BŁĘDU??
                                                                   //BucketItems = (ICollection<BucketItem>)seller, // DOBRZE PRZYPISUJE, ALE WYWALA BŁĄD ŻĄDANIA
                                    CreationDate = DateTime.UtcNow,
                                    IsAccepted = false,
                                    IsChosen = false
                                };

                                db.Transactions.Add(transaction);
                                db.SaveChanges();

                                user.BoughtTransactions.Add(transaction);
                                db.SaveChanges();

                                seller.Key.SoldTransactions.Add(transaction);
                                db.SaveChanges();
                                //ConcurencyHandling.SaveChangesWithConcurencyHandling(db);

                                //foreach (var item in seller)
                                //{
                                //    item.Transaction.Add(transaction);
                                //    ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                                //}

                                //ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                                //WyswietlanieTransakcji(transaction);

                                //foreach (var offer in seller.ToList())
                                //{
                                //    offer.Transaction.Add(transaction);
                                //}
                                //ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                                //foreach (var offer in seller.ToList())
                                //{
                                //    offer.Transaction.Add(transaction);
                                //}
                                //ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
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
                                try
                                {
                                    user.Bucket.BucketItems.Remove(item);
                                    db.SaveChanges();

                                    //Offer itemOffer = item.Offer;
                                    //itemOffer.BucketItems.Remove(item);
                                    //db.SaveChanges();

                                    //db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                                    //db.SaveChanges();

                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine("--------------------- ERROR: " + ex.InnerException.Message);
                                }

                                //db.BucketItems.Remove(item);
                                //await RemoveFromBucket(item.Offer != null ? "Offer" : "Bundle", item.Offer != null ? item.Offer.OfferID : item.Bundle.BundleID);
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

        private void WyswietlanieTransakcji()
        {
            var transactions = db.Transactions.ToList();
            foreach (var transaction in transactions)
            {
                Debug.WriteLine(transaction.BucketItems.Count());
                Debug.WriteLine(transaction.TransactionID);

                foreach (var item in transaction.BucketItems)
                {
                    Debug.WriteLine(item.Quantity);
                    Debug.WriteLine(item.TotalPrice);
                    Debug.WriteLine(item.BucketItemID);
                    Debug.WriteLine(item.Offer != null ? item.Offer.Title : item.Bundle.Title);
                    Debug.WriteLine("------ WEWN FOREACH ------");
                }
                Debug.WriteLine("------ ZEWN FOREACH ------");
            }

        }

        private void WyswietlanieTransakcji(Transaction transaction)
        {
            Debug.WriteLine("TransactionID: " + transaction.TransactionID);
            Debug.WriteLine("Transaction Bucket items count: " + transaction.BucketItems.Count());

            foreach (var item in transaction.BucketItems)
            {
                Debug.WriteLine("BucketItem ID: " + item.BucketItemID);
                Debug.WriteLine("BucketItem title: " + item.Offer != null ? item.Offer.Title : item.Bundle.Title);
                Debug.WriteLine("BucketItem quantity: " + item.Quantity);
                Debug.WriteLine("BucketItem total price: " + item.TotalPrice);
            }
        }

        #endregion




    }
}