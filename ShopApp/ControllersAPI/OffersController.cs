using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using ShopApp.DAL;
using ShopApp.Models;
using ShopApp.Utility;

namespace ShopApp.ControllersAPI
{
    public class OffersController : ApiController
    {
        private ShopContext db = new ShopContext();

        public IHttpActionResult GetOffers()
        {
            var offers = db.Offers;
            List<OfferItem> offerItems = new List<OfferItem>();
            foreach(Offer offer in offers)
            {
               var offerItem = OfferItem.ConvertOfferToOfferItem(offer);
               offerItems.Add(offerItem);
                
            }
            return Ok(offerItems);
        }


        public IHttpActionResult GetOffersByCategory(int id)
        {
            var offers = db.Offers;
            List<OfferItem> offerItems = new List<OfferItem>();
            foreach (Offer offer in offers)
            {
                if (id == offer.Category.CategoryID)
                {
                    var offerItem = OfferItem.ConvertOfferToOfferItem(offer);
                    offerItems.Add(offerItem);
                }
          
            }
            return Ok(offerItems);
        }

        [System.Web.Http.Route("Add")]
        public IHttpActionResult Add()
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                }
            }
            else
            {

            }
            //User user = db.Users.Where(u => u.UserID == model.UserID).FirstOrDefault();

            //int categoryID = model.CategoryID;
            //Category offerCategory = db.Categories.Where(o => o.CategoryID == categoryID).FirstOrDefault();

            //string priceWithDot = model.Price.Contains(',') ? model.Price.Replace(',', '.') : model.Price;

            //Offer offer = new Offer
            //{
            //    Title = model.Title,
            //    Description = model.Description,
            //    InStockOriginaly = model.InStockOriginaly,
            //    Price = Convert.ToDouble(priceWithDot),
            //    Category = offerCategory,
            //    User = user,
            //    IsActive = true,
            //    CreationDate = DateTime.Now
            //};
            //offer.InStockNow = offer.InStockOriginaly;

            //db.Offers.Add(offer);
            //db.SaveChanges();

            //List<FileResult> filesForOffer = null;
            //if (model.files != null)
            //    filesForOffer = model.files;

            //if (filesForOffer != null && filesForOffer.Count > 0 && offer != null)
            //{
            //    var files = filesForOffer;

            //    if (files != null && files.Count > 0)
            //    {
            //        try
            //        {
            //            for (int i = 0; i < files.Count; i++)
            //            {
            //                var workFile = files[i];

            //                //var fileUrl = await FileManager.UploadOfferImage(workFile, offer.OfferID, i);

            //                //if (fileUrl != null)
            //                //{
            //                //    OfferPicture offerPicture = new OfferPicture() { PathToFile = fileUrl, Offer = offer };
            //                //    pictures.Add(offerPicture);
            //                //}
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            //ViewBag.Error = "Wystąpił błąd: " + ex.Message.ToString();
            //        }
            //    }
            //    else if (files.Count == 0)
            //    {
            //        //OfferPicture offerPicture = new OfferPicture() { PathToFile = "../../Images/product.jpg", Offer = offer };
            //        //pictures.Add(offerPicture);
            //    }


            //}



            ////if (offer != null)
            ////{
            return Ok();
            ////}
            ////return BadRequest("Couldn't create offer");
          

        }







        private bool OfferExists(int id)
        {
            return db.Offers.Count(e => e.OfferID == id) > 0;
        }
    }

    public class OfferBindingModel
    {
        public string Title { get; set; }

        public string Description { get; set; }
        public string Price { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
        public double InStockOriginaly { get; set; }

        public List<FileResult> files { get; set; }

}

    public class OfferItem
    {

        public int OfferID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsActive { get; set; }

        public string Stocking { get; set; }

        public double InStockOriginaly { get; set; }

        public double InStockNow { get; set; }

        public double Price { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
       
        public List<OfferItemPicture> OfferPictures { get; set; }





        public OfferItem(int offerID, string title, string description, DateTime creationDate, bool isActive, string stocking, double inStockOriginaly, double inStockNow, double price, int userID, int categoryID,  List<OfferItemPicture> offerItemPictures)
        {
            OfferID = offerID;
            Title = title;
            Description = description;
            CreationDate = creationDate;
            IsActive = isActive;
            Stocking = stocking;
            InStockOriginaly = inStockOriginaly;
            InStockNow = inStockNow;
            Price = price;
            UserID = userID;
            CategoryID = categoryID;
            OfferPictures = offerItemPictures;
        }

        

        public static OfferItem ConvertOfferToOfferItem(Offer offer)
        {
            List<OfferItemPicture> offerItemPictures = new List<OfferItemPicture>();
            foreach (OfferPicture offerPicture in offer.OfferPictures)
            {
                var offerItemPicture = OfferItemPicture.ConvertOfferPictureToOfferItemPicture(offerPicture);
                offerItemPictures.Add(offerItemPicture);
            }

            return new OfferItem(offer.OfferID,offer.Title, offer.Description, offer.CreationDate, offer.IsActive,offer.Stocking,offer.InStockOriginaly,offer.InStockNow,offer.Price,offer.User.UserID,offer.Category.CategoryID,  offerItemPictures);
        }

    }

    public class OfferItemPicture
    {
        public int OfferImageID { get; set; }
        public string PathToFile { get; set; }
        public int OfferID { get; set; }

        public OfferItemPicture(int offerImageID, string pathToFile, int offerID)
        {
            OfferImageID = offerImageID;
            PathToFile = pathToFile;
            OfferID = offerID;
        }

        public static OfferItemPicture ConvertOfferPictureToOfferItemPicture(OfferPicture offerPicture)
        {
            return new OfferItemPicture(offerPicture.OfferImageID, offerPicture.PathToFile, offerPicture.Offer.OfferID);
        }

    }
}