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
    }
}