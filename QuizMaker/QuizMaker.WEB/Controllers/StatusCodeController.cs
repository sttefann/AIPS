using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizMaker.WEB.Controllers
{
    public class StatusCodeController : Controller
    {
        public ActionResult Index(int statusCode, Exception exception)
        {
            if (statusCode == 404)
                return RedirectToAction("Status404");
            ////if (statusCode == 403)
            ////    return RedirectToAction("Status403");
            if (statusCode == 1404)
                return RedirectToAction("Status1404");
            if (statusCode == 500)
                return RedirectToAction("Status500");
            ViewBag.status = statusCode;
            return View();
        }
        public ActionResult Status404()
        {
            return View();
        }
        public ActionResult Status403()
        {

            return View();
        }
        public ActionResult Status500()
        {
            return View();
        }
        public ActionResult Status1404()
        {

            return View();
        }
        public ActionResult Status600()
        {
            return View();
        }
    }
}