using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopApp.Models;

namespace ShopApp.ViewModels
{
    public class OrderHistoryViewModel
    {
        public IEnumerable<Transaction> Sold { get; set; }
        public IEnumerable<Transaction> Bought { get; set; }
    }
}