﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ShopApp.DAL;
using ShopApp.Models;

namespace ShopApp.ControllersAPI
{
    public class OffersController : ApiController
    {
        private ShopContext db = new ShopContext();

        // GET: api/Offers
        public IQueryable<Offer> GetOffers()
        {
            return db.Offers;
        }


        // GET: api/Offers/Category/5
        [Route("Category")]
        public IHttpActionResult GetOffersByCategory(int id )
        {
            var offers = db.Offers;
            List<OfferItem> offerItems = new List<OfferItem>();
            foreach(Offer offer in offers)
            {
                if(offer.Category.CategoryID == id)
                {
                    var offerItem = OfferItem.ConvertOfferToOfferItem(offer);
                    offerItems.Add(offerItem);
                }
            }
            return Ok(offerItems);
        }



        // GET: api/Offers/5
        [ResponseType(typeof(Offer))]
        public IHttpActionResult GetOffer(int id)
        {
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return NotFound();
            }

            return Ok(offer);
        }



        private bool OfferExists(int id)
        {
            return db.Offers.Count(e => e.OfferID == id) > 0;
        }
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
        public int BundleID { get; set; }
        public List<OfferItemPicture> OfferPictures { get; set; }





        public OfferItem(int offerID, string title, string description, DateTime creationDate, bool isActive, string stocking, double inStockOriginaly, double inStockNow, double price, int userID, int categoryID, int bundleID, List<OfferItemPicture> offerItemPictures)
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
            BundleID = bundleID;
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

            return new OfferItem(offer.OfferID,offer.Title, offer.Description, offer.CreationDate, offer.IsActive,offer.Stocking,offer.InStockOriginaly,offer.InStockNow,offer.Price,offer.User.UserID,offer.Category.CategoryID, offer.Bundle.BundleID, offerItemPictures);
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