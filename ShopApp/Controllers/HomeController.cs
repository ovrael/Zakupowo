using ShopApp.DAL;
using ShopApp.Models;
using ShopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using ShopApp.Utility;

namespace ShopApp.Controllers
{
	[AllowAnonymous]
	public class HomeController : Controller
	{
		private ShopContext db = new ShopContext();
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public ActionResult Kat(int KatID = 1)//We come here from
		{
			if (KatID < 1 || KatID > 14)
				return new HttpStatusCodeResult(404);
			//Filters logic
			var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
			OffersAndBundles offersAndBundles = new OffersAndBundles();

			var chosenCategory = db.Categories.Where(c => c.CategoryID == KatID).FirstOrDefault();
			if (chosenCategory != null)
				ViewBag.CategoryName = chosenCategory.CategoryName;

			var Offers = db.Offers.Where(i => i.Category.CategoryID == KatID && i.IsActive).ToList();

			if (Offers != null)
			{
				var OffersFiltered = Offers
					.OrderByDescending(i => i.CreationDate)
					.Take(20)
					.ToList();
				offersAndBundles.Offers = OffersFiltered;
				if (user != null)
				{
					var FavouriteOffers = user.FavouriteOffer.Where(i => i.Offer != null).Select(i => i.Offer).ToList();
					offersAndBundles.FavouriteOffersIDs = FavouriteOffers
						.Where(i => i.IsActive && OffersFiltered.Contains(i))
						.Select(i => i.OfferID);
					if (user.Bucket.BucketItems != null)
					{
						offersAndBundles.InBucketOffersIDs = user.Bucket.BucketItems.Where(i => i.Offer != null)
							.Select(i => i.Offer.OfferID).ToList();
					}
				}
			}
			else
				ViewBag.Message = "Brak ofert dla podanych filtrów";

			var Bundles = db.Bundles.Where(i => i.Offers.Where(x => x.Category.CategoryID == KatID).Any() && i.IsActive).ToList();

			if (Bundles != null)
			{
				var BundlesFiltered = Bundles.OrderByDescending(i => i.CreationDate)
									.Take(20)
									.ToList();
				offersAndBundles.Bundles = BundlesFiltered;
				if (user != null)
				{
					var FavouriteBundles = user.FavouriteOffer.Where(i => i.Bundle != null).Select(i => i.Bundle).ToList();
					if (FavouriteBundles != null)
					{
						offersAndBundles.FavouriteBundlesIDs = FavouriteBundles
							.Where(i => i.IsActive && BundlesFiltered.Contains(i))
							.Select(i => i.BundleID);
						if (user.Bucket.BucketItems != null)
							offersAndBundles.InBucketBundlesIDs = user.Bucket.BucketItems.Where(i => i.Bundle != null)
								.Select(i => i.Bundle.BundleID).ToList();
					}
				}
			}
			else
				ViewBag.Message = "Brak zestawów dla podanych filtrów";

			return View(offersAndBundles);
		}

		[HttpPost]
		public ActionResult Kat(FormCollection collection, int KatID = 1)//We come here from
		{
			var chosenCategory = db.Categories.Where(c => c.CategoryID == KatID).FirstOrDefault();
			if (chosenCategory != null)
				ViewBag.CategoryName = chosenCategory.CategoryName;

			//Filters logic
			var user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();
			OffersAndBundles offersAndBundles = new OffersAndBundles();

			int OffersAmount = db.Offers.Where(i => i.IsActive && i.Bundle == null).Count();
			int BundlesAmount = db.Bundles.Where(i => i.IsActive).Count();

			offersAndBundles.MaxPage = OffersAmount > BundlesAmount ? (OffersAmount / 20) + 1 : (BundlesAmount / 20) + 1;

			int Page = int.TryParse(collection["page"], out int page) && page > 0 ? page : 1;

			#region PRICE FILTER
			string lowestPrice = "";
			string highestPrice = "";

			if (collection["price"] != null)
			{
				string[] range = collection["price"].Replace(" ", "").Split('-');
				lowestPrice = range[0].Replace("zł", "");
				highestPrice = range[1].Replace("zł", "");
			}

			int startingPriceFilter = int.TryParse(lowestPrice, out int startingPrice) ? startingPrice : 0;
			int endingPriceFilter = int.TryParse(highestPrice, out int endingPrice) ? endingPrice : 1000;
			#endregion

			#region STATE FILTER

			foreach (var key in collection.AllKeys)
			{
				var value = collection[key];
				Debug.WriteLineIf(value != null, "Kolejna wartość dla: " + key + "\t to:" + value);
			}

			List<OfferState> possibleStates = new List<OfferState>();

			if (!string.IsNullOrEmpty(collection["stateNew"]))
			{
				string isNewString = collection["stateNew"];
				Debug.WriteLine("StateNew nie jest NULL " + isNewString);
				if (Convert.ToBoolean(isNewString))
					possibleStates.Add(OfferState.Nowy);
			}

			if (!string.IsNullOrEmpty(collection["stateUsed"]))
			{
				string isUsedString = collection["stateUsed"];
				Debug.WriteLine("StateUsed nie jest NULL! " + isUsedString);
				if (Convert.ToBoolean(isUsedString))
					possibleStates.Add(OfferState.Używany);
			}

			if (!string.IsNullOrEmpty(collection["stateDamaged"]))
			{
				string isDamagedString = collection["stateDamaged"];
				Debug.WriteLine("StateDamaged nie jest NULL! " + isDamagedString);

				if (Convert.ToBoolean(isDamagedString))
					possibleStates.Add(OfferState.Uszkodzony);
			}

			Debug.WriteLine("Tyle chce stanów: " + possibleStates.Count);

			#endregion

			var Offers = db.Offers.Where(i => i.Category.CategoryID == KatID && i.IsActive).ToList();

			if (Offers != null)
			{
				var OffersFiltered = Offers
					.Where(i => i.Price > startingPrice && i.Price < endingPrice && possibleStates.Contains(i.OfferState))
					.OrderByDescending(i => i.CreationDate)
					.Skip(20 * (Page - 1))
					.Take(20)
					.ToList();

				offersAndBundles.Offers = OffersFiltered;

				if (user != null)
				{
					var FavouriteOffers = user.FavouriteOffer.Where(i => i.Offer != null).Select(i => i.Offer).ToList();
					offersAndBundles.FavouriteOffersIDs = FavouriteOffers
						.Where(i => i.IsActive && OffersFiltered.Contains(i))
						.Select(i => i.OfferID);
					if (user.Bucket.BucketItems != null)
					{
						offersAndBundles.InBucketOffersIDs = user.Bucket.BucketItems.Where(i => i.Offer != null)
							.Select(i => i.Offer.OfferID).ToList();
					}
				}
			}
			else
				ViewBag.Message = "Brak ofert dla podanych filtrów.\n";

			var Bundles = db.Bundles.Where(i => i.Offers.Where(x => x.Category.CategoryID == KatID).Any() && i.IsActive).ToList();

			var BundlesFiltered = Bundles.Where(i => i.BundlePrice > startingPrice && i.BundlePrice < endingPrice)
									.OrderByDescending(i => i.CreationDate)
									.Skip(20 * (page - 1))
									.Take(20)
									.ToList();

			if (BundlesFiltered != null)
			{
				offersAndBundles.Bundles = BundlesFiltered;
				if (user != null)
				{
					var FavouriteBundles = user.FavouriteOffer.Where(i => i.Bundle != null).Select(i => i.Bundle).ToList();
					if (FavouriteBundles != null)
					{
						offersAndBundles.FavouriteBundlesIDs = FavouriteBundles
							.Where(i => i.IsActive && BundlesFiltered.Contains(i))
							.Select(i => i.BundleID);
						if (user.Bucket.BucketItems != null)
							offersAndBundles.InBucketBundlesIDs = user.Bucket.BucketItems.Where(i => i.Bundle != null)
								.Select(i => i.Bundle.BundleID).ToList();
					}
				}
			}
			else
				ViewBag.Message += "Brak zestawów dla podanych filtrów.\n";

			return View(offersAndBundles);
		}

		[HttpPost]
		public ActionResult Search()
		{
			string query = Request["searchText"];
			User user = db.Users.Where(i => i.Login == HttpContext.User.Identity.Name).FirstOrDefault();

			Debug.WriteLineIf(query != null, "Query: " + query);

			OffersAndBundles searchResult = new OffersAndBundles();
			if (query != null && query.Trim().Length > 0 && query != string.Empty)
			{
				Debug.WriteLine("SZUKAM OFERT I ZESTAWÓW");
				ViewBag.QueryText = query.Trim();
				string queryText = query.Trim().ToLower();

				var foundOffers = db.Offers.Where(o => o.Title.Contains(queryText) || o.Description.Contains(queryText)).OrderByDescending(o => o.CreationDate).Take(20).ToList();
				var foundBundles = db.Bundles.Where(b => b.Title.Contains(queryText)).OrderByDescending(o => o.CreationDate).Take(20).ToList();
				searchResult.Offers = foundOffers;
				searchResult.Bundles = foundBundles;

				if (user != null)
				{
					var favouriteOffers = user.FavouriteOffer.Where(i => i.Offer != null).Select(i => i.Offer).ToList();
					searchResult.FavouriteOffersIDs = favouriteOffers
						.Where(i => i.IsActive && foundOffers.Contains(i))
						.Select(i => i.OfferID);
					if (user.Bucket.BucketItems != null)
					{
						searchResult.InBucketOffersIDs = user.Bucket.BucketItems.Where(i => i.Offer != null)
							.Select(i => i.Offer.OfferID).ToList();
					}

					var favouriteBundles = user.FavouriteOffer.Where(i => i.Bundle != null).Select(i => i.Bundle).ToList();
					if (favouriteBundles != null)
					{
						searchResult.FavouriteBundlesIDs = favouriteBundles
							.Where(i => i.IsActive && foundBundles.Contains(i))
							.Select(i => i.BundleID);
						if (user.Bucket.BucketItems != null)
							searchResult.InBucketBundlesIDs = user.Bucket.BucketItems.Where(i => i.Bundle != null)
								.Select(i => i.Bundle.BundleID).ToList();
					}

				}
			}


			return View(searchResult);

		}
	}
}