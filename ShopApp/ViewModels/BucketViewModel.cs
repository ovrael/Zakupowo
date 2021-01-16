using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopApp.Models;

namespace ShopApp.ViewModels
{
    public class BucketViewModel
    {
        public IEnumerable<IGrouping<Models.User,BucketItem>> GroupedBucketItems{get;set;}

        public List<string> InActiveBucketItems { get; set; }
    }
}