using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopApp.Models;

namespace ShopApp.ViewModels
{
    public class OffersAndBundles
    {
        public IEnumerable<Offer> Offers { get; set; }
        public IEnumerable<Bundle> Bundles { get; set; }
        public IEnumerable<int> FavouriteOffersIDs { get; set; }
        public IEnumerable<int> FavouriteBundlesIDs { get; set; }
        public IEnumerable<int> InBucketOffersIDs { get; set; }
        public IEnumerable<int> InBucketBundlesIDs { get; set; }
        public int MaxPage { get; set; }
    }
}