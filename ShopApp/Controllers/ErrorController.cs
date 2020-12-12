using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopApp.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        [HandleError]
        public ActionResult NotFound()
        {
            return View();
        }
    }
}