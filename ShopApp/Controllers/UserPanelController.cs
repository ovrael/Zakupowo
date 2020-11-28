using System;
using System.IO;
using System.Net;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Security;
using System.Diagnostics;
using System.Web.Security;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Collections.Generic;
using ShopApp.DAL;
using ShopApp.Models;
using ShopApp.Utility;
using ShopApp.ViewModels;
using ShopApp.ViewModels.User;
using Antlr.Runtime.Tree;
using Microsoft.Ajax.Utilities;

namespace ShopApp.Controllers
{
    [Authorize]
    public class UserPanelController : Controller
    {
        private ShopContext db = new ShopContext();

        [Authorize]
        #region UserData 

        // VIEW WITH BASIC INFORMATION ABOUT USER
        public ActionResult Account()
        {
            User showUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();
            //FileManager.Configure();
            return View(showUser);
        }

        // VIEW WHERE USER CAN EDIT *BASIC* INFORMATION
        public ActionResult EditBasicInfo()
        {
            User showUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();

            return View(showUser);
        }

        [HttpPost]
        public ActionResult EditBasicInfo(FormCollection collection)
        {
            User editUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();

            if (editUser != null)
            {
                string changedFirstName = collection["FirstName"].Trim();
                string changedLastName = collection["LastName"].Trim();
                string changedEmail = collection["Email"].Trim();
                string changedPhoneNumber = collection["Phone"].Trim();


                if (changedFirstName != editUser.FirstName && changedFirstName != null)
                    editUser.FirstName = changedFirstName;

                if (changedLastName != editUser.LastName && changedLastName != null)
                    editUser.LastName = changedLastName;

                if (changedEmail != editUser.Email && changedEmail != null)
                    editUser.Email = changedEmail;

                if (changedPhoneNumber != editUser.Phone && changedPhoneNumber != null)
                    editUser.Phone = changedPhoneNumber;

                db.Entry(editUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("EditBasicInfo", "UserPanel");
        }

        #region ShippingAdresses
        // VIEW WHERE USER CAN EDIT SHIPPING ADRESSES
        public ActionResult ShippingAdresses()
        {
            User showUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();

            return View(showUser);
        }

        [HttpPost]
        public ActionResult ShippingAdresses(FormCollection collection)
        {
            User editUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();
            ShippingAdress shippingAdress;

            if (editUser != null)
            {
                int adressNumber = int.Parse(collection["AdressNumber"].Trim());
                shippingAdress = editUser.ShippingAdresses.ToList()[adressNumber];

                string changedCountry = collection["Country"].Trim();
                string changedCity = collection["City"].Trim();
                string changedStreet = collection["Street"].Trim();
                string changedPremisesNumber = collection["PremisesNumber"].Trim();
                string changedPostalCode = collection["PostalCode"].Trim();

                if (changedCountry != shippingAdress.Country && changedCountry != null)
                    shippingAdress.Country = changedCountry;

                if (changedCity != shippingAdress.City && changedCity != null)
                    shippingAdress.City = changedCity;

                if (changedStreet != shippingAdress.Street && changedStreet != null)
                    shippingAdress.Street = changedStreet;

                if (changedPremisesNumber != shippingAdress.PremisesNumber && changedPremisesNumber != null)
                    shippingAdress.PremisesNumber = changedPremisesNumber;

                if (changedPostalCode != shippingAdress.PostalCode && changedPostalCode != null)
                    shippingAdress.PostalCode = changedPostalCode;


                editUser.ShippingAdresses.ToList()[adressNumber] = shippingAdress;

                //db.Entry(editUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("ShippingAdresses", "UserPanel");
        }

        public ActionResult AddShippingAdress()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddShippingAdress(FormCollection collection)
        {
            User editUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();

            string country = collection["Country"];
            string city = collection["City"];
            string street = collection["Street"];
            string premisesNumber = collection["PremisesNumber"];
            string postalCode = collection["PostalCode"];

            ShippingAdress adress = new ShippingAdress
            {
                Country = country,
                City = city,
                Street = street,
                PremisesNumber = premisesNumber,
                PostalCode = postalCode,
                User = editUser
            };

            db.ShippingAdresses.Add(adress);
            db.SaveChanges();

            editUser.ShippingAdresses.Add(adress);
            db.SaveChanges();

            return RedirectToAction("ShippingAdresses", "UserPanel");
        }

        public ActionResult DeleteShippingAdress(int? adressNumber)
        {
            if (adressNumber == null)
                return RedirectToAction("ShippingAdresses", "UserPanel");

            User editUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();
            int userID = editUser.UserID;

            ShippingAdress adressToRemove = db.ShippingAdresses.Where(u => u.User.UserID == userID).ToList()[(int)adressNumber];

            editUser.ShippingAdresses.Remove(adressToRemove);
            db.ShippingAdresses.Remove(adressToRemove);
            db.SaveChanges();


            return RedirectToAction("ShippingAdresses", "UserPanel");
        }
        #endregion

        // VIEW WHERE USER CAN EDIT PASSWORD
        public ActionResult EditPassword()
        {
            User showUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();

            return View(showUser);
        }

        [HttpPost]
        public async Task<ActionResult> EditPassword(FormCollection collection)
        {
            User editUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();

            if (editUser != null)
            {
                string encryptedOldPassword = Cryptographing.Encrypt(collection["OldPassword"].Trim());
                string encryptedNewPassword = Cryptographing.Encrypt(collection["NewPassword"].Trim());
                string encryptedNewPasswordValidation = Cryptographing.Encrypt(collection["NewPasswordValidation"].Trim());

                // If written current password is the same as current password AND written current and new passwords are not NULLs
                if (encryptedOldPassword.Equals(editUser.EncryptedPassword) && encryptedOldPassword != null && encryptedNewPassword != null)
                {
                    // If new password validates and is different from old one => CHANGE PASSWORD
                    if (encryptedNewPassword.Equals(encryptedNewPasswordValidation) && !encryptedNewPassword.Equals(encryptedOldPassword))
                    {
                        // WYŚLIJ EMAIL Z POTWIERDZENIEM
                        editUser.EncryptedPassword = encryptedNewPassword;
                        db.SaveChanges();

                        await EmailManager.SendEmailAsync(EmailManager.EmailType.ChangePassword, editUser.FirstName, editUser.LastName, editUser.Email);
                    }
                }
            }

            return RedirectToAction("Account", "UserPanel");
        }

        // VIEW WHERE USER CAN EDIT AVATAR
        public ActionResult EditAvatar()
        {
            User showUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();

            return View(showUser);
        }

        [HttpPost]
        public async Task<ActionResult> EditAvatar(HttpPostedFileBase file)
        {
            User editUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();

            var imageUrl = await FileManager.UploadAvatar(file, editUser.UserID);

            if (imageUrl != null)
            {
                if (editUser.AvatarImage.PathToFile == null)
                {
                    AvatarImage newAvatar = new AvatarImage() { PathToFile = imageUrl, User = editUser };
                    db.Entry(newAvatar).State = System.Data.Entity.EntityState.Added;
                }
                else
                {
                    editUser.AvatarImage.PathToFile = imageUrl;
                    db.Entry(editUser).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                ViewBag.Message = "File uploaded successfully";
            }
            else
            {
                Debug.WriteLine("NIE UDAŁO SIĘ ZUPLOADOWAĆ PLIKU");
            }

            return RedirectToAction("EditAvatar", "UserPanel");
        }

        #endregion

        public ActionResult AddOffer()
        {
            List<Category> categoryList = db.Categories.ToList();

            return View(categoryList);
        }

        [HttpPost]
        public async Task<ActionResult> AddOffer(FormCollection collection)
        {
            User editUser = db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();

            int categoryID = int.Parse(collection["Category"]);
            Category offerCategory = db.Categories.Where(o => o.CategoryID == categoryID).FirstOrDefault();

            Offer offer = new Offer
            {
                Title = collection["Name"],
                Description = collection["Description"],
                InStockOriginaly = Convert.ToDouble(collection["Quantity"]),
                Price = Convert.ToDouble(collection["Price"]),
                Category = offerCategory,
                User = editUser,
                IsActive = true
            };
            offer.InStockNow = offer.InStockOriginaly;

            db.Offers.Add(offer);
            db.SaveChanges();

            offer = db.Offers.ToList().Last(); // DO POPRAWY

            List<OfferPicture> pictures = new List<OfferPicture>();

            var files = Request.Files;
            if (files != null && files.Count > 0)
            {
                try
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        var workFile = files[i];

                        var fileUrl = await FileManager.UploadOfferImage(workFile, offer.OfferID, i);

                        if (fileUrl != null)
                        {
                            OfferPicture offerPicture = new OfferPicture() { PathToFile = fileUrl, Offer = offer };
                            pictures.Add(offerPicture);

                            ViewBag.Message = "File uploaded successfully";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }

            offer.OfferPictures = pictures;
            db.SaveChanges();

            offer.Category.Offers.Add(offer);
            db.SaveChanges();

            editUser.Offers.Add(offer);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult OrderHistory()
        {
            return View();
        }

        public ActionResult Communicator()
        {
            return View();
        }
    }
}
