using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopApp.Models;

namespace ShopApp.ViewModels
{
    public class OfferIndexViewModel
    {
        public Offer Offer {get; set;}
        public bool IsInBucket { get; set; }
        public bool IsInFavourite { get; set; }
    }
}