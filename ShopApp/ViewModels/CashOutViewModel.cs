using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopApp.Models;

namespace ShopApp.ViewModels
{
    public class CashOutViewModel
    {
        public ICollection<IGrouping<ShopApp.Models.User,BucketItem>> GroupedBucketItems { get; set; }
        public IEnumerable<ShopApp.Models.ShippingAdress> ShippingAdresses { get; set; }
    }
}