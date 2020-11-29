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
        public ActionResult Index(int OfferID = 1)
        {
            var offer = db.Offers.Where(i => i.OfferID == OfferID).Single();
            if (offer.IsActive)
            {
                Offer oferta = DataBase.SearchForOffer((int)OfferID);
                return View(oferta);
            }
            //TODO MESSAGE WHY IT THREW ME AWAY FROM OFFER
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Authorize]
        public ActionResult Index(FormCollection collection)
        {
            if(int.TryParse(collection["prodId"],out int result))
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
                    db.BucketItems.Add(BucketItem);
                    db.SaveChanges();
                    offer.BucketItems.Add(BucketItem);
                    db.SaveChanges();
                    user.Bucket.BucketItems.Add(BucketItem);
                    db.SaveChanges();
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
    }
}