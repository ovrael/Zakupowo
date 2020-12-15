using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopApp.ViewModels
{
    public class CommunicatorViewModel
    {
        public List<Message> Messages { get; set; }
        public List<Models.User> UniqueUsers { get; }
    }
}