using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopApp.ViewModels
{
    public class HomeIndexView
    {
        public List<Category> CategoryList { get; set; }
        public List<Offer> OfferList { get; set; }
    }
}