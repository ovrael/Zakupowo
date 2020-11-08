using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopApp.Models;

namespace ShopApp.ViewModels
{
    public static class UserCache
    {
        private static Models.User user;

        public static Models.User CachedUser { get
            {
                if (user == null)
                {
                    user = new Models.User
                    {
                        Email = "jan@example.pl",
                        FirstName = "Jan",
                        LastName = "Kowalski",
                        UserID = 1,
                        EncryptedPassword = "9876",
                        Offers = new List<Offer>() { },
                    };
                }

                return user;
            } }
        
    }
}