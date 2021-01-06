using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopApp.Models;

namespace ShopApp.ViewModels
{
    public class BundleViewModel
    {
            public Bundle Bundle { get; set; }
            public IEnumerable<Offer> OffersList { get; set; }
            public bool IsInBucket { get; set; }
            public bool IsInFavourite { get; set; }
            
            public IEnumerable<OfferPicture> MainPictures { get; set; }
    }
}