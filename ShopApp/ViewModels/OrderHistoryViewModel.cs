using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopApp.Models;

namespace ShopApp.ViewModels
{
    public class OrderHistoryViewModel
    {
        public IEnumerable<Transaction> Sellings { get; set; }
        public IEnumerable<Transaction> Purchases { get; set; }
    }
}