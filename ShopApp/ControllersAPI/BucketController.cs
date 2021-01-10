using ShopApp.DAL;
using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ShopApp.ControllersAPI
{
    public class BucketController : ApiController
    {
        private ShopContext db = new ShopContext();
        // GET: Default
        public IHttpActionResult GetOffersByCategory(int id)
        {
            var user = db.Users.Where(i => i.UserID == id).FirstOrDefault();
            if (user != null)
            {
                var BucketItems = user.Bucket.BucketItems;

                List<BindingBucketItem> list = new List<BindingBucketItem>();
                foreach(var item in BucketItems)
                {
                    list.Add(BindingBucketItem.Convert(item));
                }
                //Consider using Critical error page for below
                if (user.ShippingAdresses.Count() == 0)
                    return Ok("Przed przejściem do kasy wymagane jest ustawienie adresu dostawy");
                if (user?.Bucket?.BucketItems == null)
                    return Ok("Żeby przejść do kasy musisz mieć jakieś przedmioty w swoim koszyku.");
                if ((bool)!user?.IsActivated)
                    return Ok("Aby dokonać zakupu konto musi być aktywowane");
                return Ok(list);
            }
            else
                //Returning 404 when somehow user is authorized but not in Database
                return BadRequest();

        }

    }

    public class BindingBucketItem 
    {
        public int BucketItemID { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public bool IsChosen { get; set; }
        public virtual OfferItem Offer { get; set; }

        public static BindingBucketItem Convert(BucketItem bucketItem)
        {
            OfferItem offer = OfferItem.ConvertOfferToOfferItem(bucketItem.Offer);
            BindingBucketItem item = new BindingBucketItem
            {
                BucketItemID = bucketItem.BucketItemID,
                Quantity = bucketItem.Quantity,
                TotalPrice = bucketItem.TotalPrice,
                IsChosen = bucketItem.IsChosen,
                Offer = offer
            };
            return item;
        }

    }

   

}