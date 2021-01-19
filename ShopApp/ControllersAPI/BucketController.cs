using Newtonsoft.Json;
using ShopApp.DAL;
using ShopApp.Models;
using ShopApp.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;


namespace ShopApp.ControllersAPI
{
    [System.Web.Http.RoutePrefix("api/Bucket")]
    public class BucketController : ApiController
    {
        private ShopContext db = new ShopContext();
        // GET: Default
        public IHttpActionResult GetBucketItems(int id)
        {
            var user = db.Users.Where(i => i.UserID == id).FirstOrDefault();
            if (user != null)
            {
                
                var BucketItems = user.Bucket.BucketItems;

                List<BindingBucketItem> list = new List<BindingBucketItem>();
                foreach (var item in BucketItems)
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



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("BuyBucketItems")]
        public async Task<IHttpActionResult> BuyBucketItemsAsync()
        {  
            string json = await Request.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            string login = data["login"];
            int addressId = Convert.ToInt32(data["addressId"]);

         
            var user = db.Users.Where(i => i.Login == login && i.IsActivated).FirstOrDefault();
            
            //Odpowiednie errorr message dlaczego
            if (user != null)
            {
                
                var order = user.Bucket?.BucketItems?.ToList();
                var Address = user.ShippingAdresses?.Where(i => i.AdressID == addressId).FirstOrDefault();
                List<BucketItem> ItemsThatCouldntBeenSold = new List<BucketItem>();
                if (order != null && Address != null)
                {
                    var grouped = order.GroupBy(i => i.Offer != null ? i.Offer.User : i.Bundle.User);
                    foreach (var seller in grouped)
                    {
                        var message = "Jestem zainteresowany zakupem wystawionego produktu proszę o odpowiedź.";
                        
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
                   
                    var BucketItemsInOrderTab = user.Bucket.BucketItems.ToArray();
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

                    user.Order?.BucketItems?.Clear();
                    ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                }
            }
            return BadRequest();
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("RemoveFromBucket")]
        public async Task<IHttpActionResult> RemoveFromBucket()
        {

            string json = await Request.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            string login = data["login"];
            int id = Convert.ToInt32(data["offerId"]);


            var user = db.Users.Where(i => i.Login == login && i.IsActivated).FirstOrDefault();
          

            if ( user != null && id > 0)
            {
                
                    var Offer = db.Offers.Where(i => i.OfferID == id).FirstOrDefault();
                    if (Offer != null)
                    {
                        BucketItem BucketItemToRemove = user.Bucket?.BucketItems?.Where(i => i.Offer != null && i.Offer.OfferID == id).FirstOrDefault();
                        if (BucketItemToRemove != null)//We chceck if user called for existing and active offer
                        {
                            user.Bucket.BucketItems.Remove(BucketItemToRemove);
                            Offer.BucketItems.Remove(BucketItemToRemove);
                            db.BucketItems.Remove(BucketItemToRemove);
                            ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                        }
                        else
                            return Ok( "Nie posiadasz tej oferty w swoim koszyku");
                    }
                    else
                    return Ok("Wybrana oferta nie istnieje");

            }
            else
            return BadRequest("Wprowadzone dane są niepoprawne");

            return BadRequest();
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
}