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
                        ViewBag.ConfirmChanges = "Potwierdź link w wysłanym mail'u by zastosować zmianę hasła.";

                        await EmailManager.SendEmailAsync(EmailManager.EmailType.ChangePassword, editUser.FirstName, editUser.LastName, editUser.Email, encryptedNewPassword);
                    }
                }
            }

            return View(editUser);
        }

        public ActionResult PasswordChange()
        {
            string userEmail = TempData["email"].ToString();

            User editUser = db.Users.Where(u => u.Email.Equals(userEmail)).FirstOrDefault();

            if (editUser != null)
            {
                string newPassword = TempData["encryptedNewPassword"].ToString();
                editUser.EncryptedPassword = newPassword;
                db.SaveChanges();

                return View(editUser);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult ConfirmPasswordChange(string email, string psw)
        {
            string userEmail = EmailManager.DecryptEmail(email);
            TempData["email"] = userEmail;
            TempData["encryptedNewPassword"] = psw;

            User editUser = db.Users.Where(u => u.Email.Equals(userEmail)).FirstOrDefault();

            if (editUser != null)
                return RedirectToAction("PasswordChange", "UserPanel");
            else
                return RedirectToAction("Index", "Home");
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
                if (editUser.AvatarImage == null)
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


        #region OffersAndBundles   

        public ActionResult Offers()
        {
            User editUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();

            List<Offer> offers = editUser.Offers.ToList();

            return View(offers);
        }

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
                IsActive = true,
                CreationDate = DateTime.Now
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
            else if (files.Count == 0)
            {
                OfferPicture offerPicture = new OfferPicture() { PathToFile = "../../Images/product.jpg", Offer = offer };
                pictures.Add(offerPicture);
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

        public ActionResult DeactivateOffer(int? offerID)
        {
            if (offerID == null)
            {
                ViewBag.Error = "offerID == null";
                return RedirectToAction("Offers", "UserPanel");
            }

            User editUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();

            db.Offers.Where(o => o.OfferID == offerID).FirstOrDefault().IsActive = false;
            db.SaveChanges();


            //foreach (var offer in bundleToRemove.Offers)
            //{
            //    offer.Bundle = null;
            //}
            //db.SaveChanges();

            //editUser.Offers.Remove(offerToDeactivate);
            //db.SaveChanges();
            //db.Offers.Remove(offerToDeactivate);
            //db.SaveChanges();

            return RedirectToAction("Offers", "UserPanel");
        }

        public ActionResult Bundles(bool? availableOffers)
        {
            User editUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();

            List<Bundle> userBundles = editUser.Bundles.ToList();

            if (availableOffers != null && availableOffers == false)
            {
                ViewBag.NoOffers = "Brak dostępnych ofert do stworzenia zestawu!";
            }

            return View(userBundles);
        }

        public ActionResult AddBundle()
        {
            User editUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();
            List<Offer> userOffers = editUser.Offers.Where(o => o.IsActive == true && o.InStockNow > 0 && o.Bundle == null).ToList();

            if (userOffers.Count > 0)
                return View(userOffers);
            else
            {
                return RedirectToAction("Bundles", "UserPanel", new { availableOffers = false });
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddBundle(FormCollection collection)
        {
            User editUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();

            Bundle newBundle = new Bundle();

            double offersPriceSum = 0.0;
            double bundlePrice = 0.0;
            List<Offer> bundleOffers = new List<Offer>();

            if(collection["OffersInBundle"] == null)
            {
                ViewBag.NoPickedOffers = "Brak przydzielonych ofert do zestawu!";
                return View();
            }


            string[] offerIDs = collection["OffersInBundle"].Split(',');
            foreach (var offerID in offerIDs)
            {
                if (int.TryParse(offerID, out int ID))
                {
                    Offer offer = db.Offers.Where(o => o.OfferID == ID).FirstOrDefault();
                    bundleOffers.Add(offer);

                    string offerQuantity = "OffersQuantity_" + offerID;
                    offersPriceSum += offer.Price * int.Parse(collection[offerQuantity]);
                }
            }

            if (collection["RadioDiscount"] == "CurrencyDiscount")
            {
                if (double.TryParse(collection["CurrencyDiscountValue"], out double discount))
                {
                    bundlePrice = offersPriceSum - discount;
                }
                else
                {
                    ViewBag.Error = "Cant parse discount to double";
                }
            }
            else if (collection["RadioDiscount"] == "PercentDiscount")
            {
                if (int.TryParse(collection["PercentDiscountValue"], out int discount))
                {
                    bundlePrice = offersPriceSum - offersPriceSum * discount / 100;
                }
                else
                {
                    ViewBag.Error = "Cant parse discount to int";
                }
            }
            else
            {
                bundlePrice = offersPriceSum;
            }

            newBundle.Title = collection["BundleTitle"];
            newBundle.Offers = bundleOffers;
            newBundle.OffersPriceSum = offersPriceSum;
            newBundle.BundlePrice = bundlePrice;
            newBundle.CreationDate = DateTime.Now;
            newBundle.IsActive = true;
            newBundle.User = editUser;

            try
            {
                db.Bundles.Add(newBundle);
                db.SaveChanges();

                editUser.Bundles.Add(newBundle);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }


            return RedirectToAction("Bundles", "UserPanel");
        }

        public ActionResult DeactivateBundle(int? bundleID)
        {
            if (bundleID == null)
            {
                ViewBag.Error = "BundleID == null";
                return RedirectToAction("Bundles", "UserPanel");
            }

            User editUser = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).First();

            db.Bundles.Where(b => b.BundleID == bundleID).FirstOrDefault().IsActive = false;
            db.SaveChanges();

            //foreach (var offer in bundleToDeactivate.Offers)
            //{
            //    offer.Bundle = null;
            //}
            //db.SaveChanges();


            //editUser.Bundles.Remove(bundleToDeactivate);
            //db.SaveChanges();
            //db.Bundles.Remove(bundleToDeactivate);
            //db.SaveChanges();

            return RedirectToAction("Bundles", "UserPanel");
        }

        #endregion

        public ActionResult OrderHistory()
        {
            return View();
        }

        public ActionResult Communicator()
        {
            return View();
        }
        public ActionResult AddProductSet()
        {
            return View();
        }
        public ActionResult ProductSet()
        {
            return View();
        }
    }
}
