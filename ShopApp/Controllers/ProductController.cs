using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ShopApp.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product/Random
        public ActionResult Random()
        {
            var product = new Product() { Name="Krzesło"};

            return View(product);
        }
    }
}