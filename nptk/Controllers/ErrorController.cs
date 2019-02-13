using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nptk.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult ServerError()
        {
            Response.StatusCode = 500;
            return View();
        }

        public ActionResult UnauthorisedRequest()
        {
            Response.StatusCode = 403;
            return View();
        }

        public ActionResult BadRequest()
        {
            Response.StatusCode = 400;
            return View();
        }

        //Any other errors you want to specifically handle here.

        public ActionResult CatchAllUrls()
        {
            //throwing an exception here pushes the error through the Application_Error method for centralised handling/logging
            throw new HttpException(404, "The requested url " + Request.Url.ToString() + " was not found");
        }
    }
}