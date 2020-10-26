﻿using Microsoft.Ajax.Utilities;
using ShopApp.DAL;
using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI;

namespace ShopApp.Controllers
{
    public class DatabaseController : Controller
    {
        //Database declaration
        private ShopContext db = new ShopContext();
        #region User database manipulation
        //Users list
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
        public ActionResult UserCreate()
        {
                return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserCreate([Bind(Include = "Email,Login,EncryptedPassword")] User Usr)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(Usr);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Offers database manipulation
        public ActionResult OffersIndex(int id)
        {
            //Checking for offers that are empty - TODO: Remove them after transaction
            var usr = db.Users.Where(i => i.UserID == id).First();
            foreach (var item in usr.Offers)
                if (item.InStock == 0)
                {
                    usr.Offers.Remove(item);
                    db.Offers.Remove(item);
                    db.SaveChanges();
                }
            return View(db.Offers.Where(i => i.User.UserID == id));
        }
        public ActionResult OfferCreate(int id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OfferCreate([Bind(Include = "Title,Stocking,InStock,Price")] Offer Offr, int id)
        {
            Offr.User = db.Users.ToList().Where(i => i.UserID == id).First();
           //TODO: Require a ModelState.IsValid
                db.Offers.Add(Offr);// First we need to add it to proper table before really using it then save
                db.SaveChanges();
                var ofr = db.Offers.Where(i => i.User.UserID == id && i.OfferID == Offr.OfferID).First();//We look for exact offer
                ofr.User.Offers.Add(ofr);// Adding the proper offer's object to a user
                db.SaveChanges();
                return RedirectToAction("Index");//Returning
         //   }
          //  return RedirectToAction("Index");
        }
        #endregion

        #region Bundle database manipulation
        public ActionResult BundleIndex(int id)
        {
            var usr = (db.Users.Where(x => x.UserID == id).FirstOrDefault());
            var Bndles = db.Bundles.Where(i => i.User.UserID == usr.UserID);
            return View(Bndles);
        }
        //Tworzenie bundli
        public ActionResult BundleCreate(int id)
        {
            return id == 0 ?  (ActionResult)RedirectToAction("Index") :  View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BundleCreate([Bind(Include = "Title")] Bundle Bundle, int id)
        {
            Bundle.Title = Bundle.Title.Trim();//Adding offer to bundle via title TODO: selecting from drop-list
            var Usr = db.Users.Where(i => i.UserID == id).First();
            var Bundles = db.Bundles.Where(i => i.User.UserID == Usr.UserID);
            if (ModelState.IsValid)
            {
                db.Bundles.Add(Bundle);
                db.SaveChanges();
                Usr.Bundles.Add(Bundle);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult AddToBundle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddToBundle(int id, string title)
        {

            var bundle = db.Bundles.Where(i => i.Title == title.Trim()).First();
            var offer = db.Offers.Where(i => i.OfferID == id).First();
            if (ModelState.IsValid)
            {
                bundle.Offers.Add(offer);
                db.SaveChanges();
            }
            return RedirectToAction("BundleIndex", new {id});
        }
        #endregion

        #region Category database manipultion
        public ActionResult AddCategory(int id)//Param is most likely a Offer's ID
        {
            return View(db.Categories);
        }




        [HttpPost]
        public ActionResult AddCategory(FormCollection collection, int id)//Param is most likely a Offer's ID
        {
            var offr = db.Offers.Where(i => i.OfferID == id).First();
            var Checked = collection["CategoryID"].Split(',');//List of checked categories
            foreach (var item in Checked)
            { 
                //TU SIE PSUJE STRASZNIE FEST
                (db.Categories.Where(i => i.CategoryID == int.Parse(item)).First())
                    .Offers.Add(db.Offers.Where(x=>x.OfferID == id).First());
                offr.Categories.Add(db.Categories.Where(i => i.CategoryID == int.Parse(item)).First());
            }
            db.SaveChanges();
            //If(ModelState.IsValid)
            int Id = db.Users.Where(i => i.Offers.First().OfferID == id).Select(i => i.UserID).First();
            return RedirectToAction("OffersIndex", "RegisterController", new { id = Id });
        }






        //public ActionResult AddToCategory(int id)
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult OfferCreate([Bind(Include = "Title,Stocking,InStock,Price")] Offer Offr, int id)
        //{
        //    Offr.User = db.Users.ToList().Where(i => i.UserID == id).First();
        //    //TODO: Require a ModelState.IsValid
        //    db.Offers.Add(Offr);// First we need to add it to proper table before really using it then save
        //    db.SaveChanges();
        //    var ofr = db.Offers.Where(i => i.User.UserID == id && i.OfferID == Offr.OfferID).First();//We look for exact offer
        //    ofr.User.Offers.Add(ofr);// Adding the proper offer's object to a user
        //    db.SaveChanges();
        //    return RedirectToAction("Index");//Returning
        //                                     //   }
        //                                     //  return RedirectToAction("Index");
        //}

        #endregion
        public ActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Delete(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            { 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
