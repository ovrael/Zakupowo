using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopApp.ViewModels.User
{
    public class AccountViewModel
    {
        public string Email { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string BirthDate { get; set; }
        public string CreationDate { get; set; }
        public List<ShippingAdressViewModel> ShippingAdresses { get; set; }

    }
}