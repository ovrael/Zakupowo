using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopApp.ViewModels.User
{
    public class ShippingAdressViewModel
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PremisesNumber { get; set; }
        public string PostalCode { get; set; }
    }
}