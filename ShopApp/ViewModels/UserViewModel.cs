using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopApp.Models;

namespace ShopApp.ViewModels
{
    public class UserViewModel
    {
        public ShopApp.Models.User user { get; set; }

        public OffersAndBundles OffersAndBundles { get; set; }
        public bool IsOwner { get; set; }
    }
}