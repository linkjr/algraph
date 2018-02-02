using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Algraph.Web.Controllers
{
    public class AuthorController : Controller
    {
        // GET: Author
        public ActionResult Index()
        {
            var model = new List<string> { "a", "b" };
            if (Request.IsAjaxRequest())
                return PartialView("_ListParrtial", model);
            else
                return View(model);
        }
    }
}