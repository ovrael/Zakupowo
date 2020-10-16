using ShopApp.Models;
using ShopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ShopApp.Controllers
{
    public class ProductController : Controller { 

        public ActionResult Random()
        {
            var product = new Product() { Name="Krzesło"};
            var customers = new List<Customer>
            {
                new Customer {Name = "Kamil Jonak"},
                new Customer {Name = "Piotr Kaczka"}
            };

            var viewModel = new RandomProductViewModel
            {
                Product = product,
                Customers = customers
            };

            return View(viewModel);
        }

    
    }
}