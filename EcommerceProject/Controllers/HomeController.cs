using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new SQLServerContext())
            {
                var publis = db.Publicaciones.Where(p => p.Visible == true && p.Estado != "Desactivada");
                if (publis.Count() > 0)
                {
                    return View(publis);
                }
                else
                {
                    return View("Error");
                }
            }
        }
    }
}