using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace ShopApp.Controllers
{
    [HandleError]
    public class ErrorController : Controller
    {
        // GET: Error
        
        public ActionResult InternalServerError()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            Debug.WriteLine(Response.StatusCode);
            return View();
        }
    }
}