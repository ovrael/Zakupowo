using ShopApp.Models;
using ShopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopApp.Controllers
{
    public class UsersController : Controller
    {
        // GET: User
        public ActionResult Users()
        {
        
            var customers = new List<Customer>
            {
                new Customer {Name = "Kamil Jonak"},
                new Customer {Name = "Piotr Kaczka"}
            };

          
            return View(customers);
        }
      
     

        public ActionResult Details(int id)
        {
            var customer = GetCustomers().SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }

        private IEnumerable<Customer> GetCustomers()
        {
            return new List<Customer>
        {
            new Customer { Id = 1, Name = "John Smith" },
            new Customer { Id = 2, Name = "Mary Williams" }
        };
            
        }
    }
}