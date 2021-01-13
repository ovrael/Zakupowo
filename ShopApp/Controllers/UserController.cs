using ShopApp.DAL;
using ShopApp.Models;
using ShopApp.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ShopApp.ViewModels;


namespace ShopApp.Controllers
{
    public class UserController : Controller
    {

        private ShopContext db = new ShopContext();


        //USER REGISTRATION
        public ActionResult Register()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            else
                return View();
        }

        //POST: Register/Create
        [HttpPost]
        public async Task<ActionResult> Register(FormCollection collection)
        {
            string email = collection["Email"].Trim();
            string login = collection["Login"].Trim();
            User tmpEmailUser = db.Users.Where(u => u.Email == email).FirstOrDefault();
            User tmpLoginUser = db.Users.Where(u => u.Login == login).FirstOrDefault();

            bool properDate = DateTime.TryParse(collection["BirthDate"], out DateTime dataUrodzenia);
            bool properAge = Utilities.CheckRegistrationAge(dataUrodzenia);
            bool uniqueEmail = tmpEmailUser is null; // If user with given EMAIL doesn't exist returns true that allows to register, works like "tmpEmailUser is null ? true : null"
            bool uniqueLogin = tmpLoginUser is null; // If user with given LOGIN doesn't exist returns true that allows to register, works like "tmpLoginUser is null ? true : null"

            if (!properDate) ViewBag.DateMessage = "Invalid date.";
            if (!properAge) ViewBag.AgeMessage = "You must be at least 13 years old.";
            if (!uniqueEmail) ViewBag.EmailMessage = "Account with that email already exists!";
            if (!uniqueLogin) ViewBag.LoginMessage = "Account with that login already exists!";

            if (ModelState.IsValid && properDate && properAge && uniqueEmail && uniqueLogin)
            {
                User user = new User()
                {
                    FirstName = collection["FirstName"].Trim(),
                    LastName = collection["LastName"].Trim(),
                    Login = login,
                    EncryptedPassword = Cryptographing.Encrypt(collection["Password"]),
                    Email = email,
                    BirthDate = dataUrodzenia,
                    CreationDate = DateTime.Now,
                    IsActivated = false
                };
                AvatarImage avatarImage = new AvatarImage() { PathToFile = "../../App_Files/Images/UserAvatars/DefaultAvatar.jpg", User = user };

                db.Users.Add(user);
                db.SaveChanges();

                user.AvatarImage = avatarImage;
                db.SaveChanges();

                var bucket = new Bucket
                {
                    User = db.Users.Where(i => i.Login == user.Login).First()
                };
                db.Buckets.Add(bucket);
                db.SaveChanges();

                Order UniqueOrderForThatUser = new Order
                {
                    User = db.Users.Where(i => i.Login == user.Login).First()
                };

                db.Orders.Add(UniqueOrderForThatUser);
                db.SaveChanges();


                Task.Run(() => EmailManager.SendEmailAsync(EmailManager.EmailType.Registration, user.FirstName, user.LastName, user.Email));
                return RedirectToAction("Login");
            }
            return View();
        }


        public ActionResult ActivateAccount()
        {
            string userEmail = TempData["email"].ToString();
            User editUser = db.Users.Where(u => u.Email.Equals(userEmail)).FirstOrDefault();

            if (editUser != null)
            {
                editUser.IsActivated = true;
                db.SaveChanges();


                return View(editUser);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult ConfirmRegistration(string email)
        {
            string userEmail = EmailManager.DecryptEmail(email);
            TempData["email"] = userEmail;

            User editUser = db.Users.Where(u => u.Email.Equals(userEmail)).FirstOrDefault();

            if (editUser != null)
                return RedirectToAction("ActivateAccount", "User");
            else
                return RedirectToAction("Index", "Home");
        }

        //Login methods
        public ActionResult Login()
        {

            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            else
                return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var userIdenticator = collection["UserIdenticator"];
            var password = Cryptographing.Encrypt(collection["EncryptedPassword"]);

            User user = null;

            if (userIdenticator != null && userIdenticator.Contains("@"))
                user = db.Users.Where(x => x.Email == userIdenticator && x.EncryptedPassword == password).SingleOrDefault();
            else
                user = db.Users.Where(x => x.Login == userIdenticator && x.EncryptedPassword == password).SingleOrDefault();

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Login, (collection["rememberMeInput"] == "rememberMe"));

                if (user.AvatarImage == null)
                {
                    Debug.WriteLine("Brak zdjęcia dodam defaultowe");
                    AvatarImage avatarImage = new AvatarImage() { PathToFile = "../../App_Files/Images/UserAvatars/DefaultAvatar.jpg", User = user };

                    user.AvatarImage = avatarImage;
                    db.SaveChanges();
                }

                ViewBag.UserAvatarURL = user.AvatarImage.PathToFile;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Nieprawidłowe dane logowania";
            return View();
        }

        [HttpGet]
        public ActionResult PasswordResetRequest()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
                return new HttpStatusCodeResult(404);
        }
        [HttpPost]
        public ActionResult PasswordResetRequest(FormCollection collection)
        {
            if (collection != null && !User.Identity.IsAuthenticated)
            {
                if (collection["email-input"] != null && collection["email-input"].Contains('@'))
                {
                    string Email = collection["email-input"];
                    string PasswordResetCode = ShopApp.Utility.Utilities.RandomString(8);
                    var user = db.Users.Where(i => i.Email == Email).FirstOrDefault();
                    
                    if (user != null)
                    {
                        Task.Run(() => EmailManager.SendEmailAsync(user.FirstName, user.LastName, Email, PasswordResetCode));

                        PasswordReset ResetCode = new PasswordReset(Email,PasswordResetCode);


                        db.PasswordResetCodes.Add(ResetCode);
                        ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                    }

                    Session["ResetPasswordEmail"] = Email;
                    return RedirectToAction("PasswordReset", "User");
                }
                else
                    ViewBag.ErrorMessage = "Prosze podać email";
            }
            else
                ViewBag.ErrorMessage = "Nieprawidłowe dane";
            return View();
        }
        [HttpGet]
        public ActionResult PasswordReset(string e)
        {
            if (Session["ResetPasswordEmail"] != null && !User.Identity.IsAuthenticated)
                return View();
            else
                return new HttpStatusCodeResult(404);
        }
        [HttpPost]
        public ActionResult PasswordReset(FormCollection collection)
        {
            if (Session["ResetPasswordEmail"] != null && !User.Identity.IsAuthenticated)
            {
                string email = Session["ResetPasswordEmail"].ToString();
                if (collection["codeInput"] != null && collection["EncryptedPassword"] != null && collection["passwordRegisterInput2"] != null)
                {
                    try
                    {
                    string Code = collection["codeInput"];
                    string NewPassword = collection["EncryptedPassword"];
                    string NewPasswordRepeated = collection["passwordRegisterInput2"];

                        var PasswordCode = db.PasswordResetCodes.Where(i => i.EmailAddress == email && i.CodeExpirationTime > DateTime.UtcNow && !i.Used).OrderByDescending(i => i.CodeCreationTime).FirstOrDefault(); 
                        var user = db.Users.Where(i => i.Email == email).FirstOrDefault();
                    
                    if(PasswordCode != null && user != null)
                    {
                        if(Code.Equals(PasswordCode.PasswordResetCode) && NewPassword.Equals(NewPasswordRepeated) && PasswordCode.TriesCount < 3)
                        {
                            user.EncryptedPassword = Cryptographing.Encrypt(NewPassword);
                            PasswordCode.Used = true;
                            ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                            Session.Clear();
                            return RedirectToAction("Login", "User");
                        }
                        else
                        {
                            PasswordCode.TriesCount++;
                            ViewBag.ErrorMessage = "Niepoprawny kod";
                           ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                            return View();
                        }
                    }
                    }catch (Exception ex)
                    {
                        Debug.WriteLine(ex.StackTrace);
                        return new HttpStatusCodeResult(500);
                    }
                    
                }
            }
                return new HttpStatusCodeResult(404);
        }

        //Logout method 
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        public ActionResult UserInformation(int? UserId)
        {
            if (UserId != null)
            {
                var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
                var ViewUser = db.Users.Where(i => i.UserID == UserId).FirstOrDefault();

                if (ViewUser != null)
                {

                    OffersAndBundles offersAndBundles = new OffersAndBundles();
                    ViewModels.UserViewModel UserViewModel = new ViewModels.UserViewModel();
                    UserViewModel.user = ViewUser;
                    var offers = db.Offers.Where(i => i.User.UserID == ViewUser.UserID).ToList();
                    var bundles = db.Bundles.Where(i => i.User.UserID == ViewUser.UserID).ToList();
                    if (user != null)
                    {
                        var FavouriteOffersIDs = user.FavouriteOffer?.Where(i => i.Offer != null && i.Offer.IsActive).Select(i => i.FavouriteOfferID).ToList();
                        var FavouriteBundlesIDs = user.FavouriteOffer?.Where(i => i.Bundle != null && i.Bundle.IsActive).Select(i => i.FavouriteOfferID).ToList();

                        var InBucketOffersIDs = user.Bucket?.BucketItems?.Where(i => i.Offer != null && i.Offer.IsActive).Select(i => i.BucketItemID).ToList();
                        var InBucketBundlesIDs = user.Bucket?.BucketItems?.Where(i => i.Bundle != null && i.Bundle.IsActive).Select(i => i.BucketItemID).ToList();

                        if (offers != null)
                            offersAndBundles.Offers = offers;
                        if (bundles != null)
                            offersAndBundles.Bundles = bundles;
                        if (FavouriteOffersIDs != null)
                            offersAndBundles.FavouriteOffersIDs = FavouriteOffersIDs;
                        if (FavouriteBundlesIDs != null)
                            offersAndBundles.FavouriteBundlesIDs = FavouriteBundlesIDs;
                        if (InBucketOffersIDs != null)
                            offersAndBundles.InBucketOffersIDs = InBucketOffersIDs;
                        if (InBucketBundlesIDs != null)
                            offersAndBundles.InBucketBundlesIDs = InBucketBundlesIDs;
                        if (user.UserID == ViewUser.UserID)
                            UserViewModel.IsOwner = user == ViewUser;
                    }
                    UserViewModel.OffersAndBundles = offersAndBundles;
                    return View(UserViewModel);
                }
            }
            return new HttpStatusCodeResult(404);
        }
    }
}
