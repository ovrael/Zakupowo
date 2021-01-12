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
        public ActionResult PreRestorePassword()
        {

            return View();
        }
        [HttpPost]
        public ActionResult PreRestorePassword(FormCollection collection)
        {

            //if (collection != null)
            //{

            //} collection["email-input"]

            //wysylamy maila
            return View();
        }
        [HttpGet]
        public ActionResult RestorePassword()
        {
            //wyswietlanie pola do wpisania kodu kodu
            return View();
        }
        [HttpPost]
        public ActionResult RestorePassword(FormCollection collection)
        {
            //collection["restore-code"]
            //collection["new-password"]
            return View();
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
			//int lastUserID = db.Users.ToList().Last().UserID;//if (userID != null && userID >= 0 && userID <= lastUserID)//{
			//    var showUser = db.Users.Where(u => u.UserID == userID).FirstOrDefault();
			//    if (showUser != null)
			//    {
			//        return View(showUser);
			//    }
			//}

			//return View();
			var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
			var ViewUser = db.Users.Where(i => i.UserID == UserId).FirstOrDefault();

			if (ViewUser != null)
			{

				OffersAndBundles offersAndBundles = new OffersAndBundles();
				var offers = ViewUser.Offers.Where(i => i.IsActive).ToList();
				var bundles = ViewUser.Bundles.Where(i => i.IsActive).ToList();

				var FavouriteOffersIDs = ViewUser.FavouriteOffer?.Where(i => i.Offer != null && i.Offer.IsActive).Select(i => i.FavouriteOfferID).ToList();
				var FavouriteBundlesIDs = ViewUser.FavouriteOffer?.Where(i => i.Bundle != null && i.Bundle.IsActive).Select(i => i.FavouriteOfferID).ToList();

				var InBucketOffersIDs = ViewUser.Bucket?.BucketItems?.Where(i => i.Offer != null && i.Offer.IsActive).Select(i => i.BucketItemID).ToList();
				var InBucketBundlesIDs = ViewUser.Bucket?.BucketItems?.Where(i => i.Bundle != null && i.Bundle.IsActive).Select(i => i.BucketItemID).ToList();

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


				ViewModels.UserViewModel UserViewModel = new ViewModels.UserViewModel()
				{
					user = ViewUser,
					OffersAndBundles = offersAndBundles
				};
				if (user != null)
					UserViewModel.IsOwner = user == ViewUser;

				return View(UserViewModel);
			}
			return new HttpStatusCodeResult(404);
		}
	}
}
