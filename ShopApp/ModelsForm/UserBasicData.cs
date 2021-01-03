using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopApp.ModelsForm
{
    public class UserBasicData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public override string ToString()
        {
            return "FirstName: " + FirstName + "\tLastName: " + LastName + "\tEmail: " + Email + "\tPhoneNumber: " + Phone;
        }
    }
}