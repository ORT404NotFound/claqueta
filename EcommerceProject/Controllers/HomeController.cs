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
                var publis = db.Publicaciones.Where(p => p.Visible == true && p.Estado != "Desactivada").ToList();
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

        public ActionResult VerDetalle(int id)
        {
            using (var db = new SQLServerContext())
            {
                var publicacion = db.Publicaciones.Where(p => p.Id == id).FirstOrDefault();
                if (publicacion != null)
                {
                    return View(publicacion);
                }
                else
                {
                    return View("Error");
                }
            }
        }

    }
}