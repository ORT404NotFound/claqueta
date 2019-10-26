using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using EcommerceProject.MPApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EcommerceProject.Controllers
{

    public class PublicationController : Controller
    {
        public ActionResult List()
        {
            using (var db = new SQLServerContext())
            {
                var publis = db.Publicaciones.Where(p => p.Visible == true && p.Estado != "Desactivada");
                if (publis.Count() > 0)
                {
                    return View(publis);
                }
                else {
                    return View("Error");
                }
            }
        }

        //muestra vista para guardar publicacion
        public ActionResult SavePublication()
        {
            if (Session["UserId"] != null)
            {
                return View();

            }
            else {
                return View("NotAuthorized");
            }
        }
        
        // para guardar una publicacion, necesita auth de user
        [HttpPost]
        public ActionResult SavePublication(Publicacion publication)
        {
            if (Session["UserId"] == null) {
                return View("NotAuthorized");
            }
            if (ModelState.IsValid) {
                int userId;
                try
                {
                    userId = Int32.Parse(Session["UserId"].ToString());
                }
                catch (FormatException e)
                {
                    throw e;
                }
                using (var db = new SQLServerContext())
                {
                    Usuario u = db.Usuarios.Find(userId);
                    publication.Usuario = u;
                    publication.Estado = "Pendiente";
                    db.Publicaciones.Add(publication);
                    db.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = "La publicacion fue guardada exitosamente";
                    return View();

                }
            }
            return View();
        }
        
        public ActionResult DisablePublication(int idPublication) {
            if (Session["UserId"] == null)
            {
                return View("NotAuthorized");
            }
            using (var db = new SQLServerContext())
            {
                Publicacion p = db.Publicaciones.Find(idPublication);
                p.Estado = "Desactivada";
                db.SaveChanges();
                return RedirectToAction("UserInfo","Account");
            }
        }

        public ActionResult PromocionarPublication(int idPublication)
        {
            if (Session["UserId"] == null)
            {
                return View("NotAuthorized");
            }
            


            using (var db = new SQLServerContext())
            {
                Publicacion p = db.Publicaciones.Find(idPublication);
                p.Promocicionada = true;
                db.SaveChanges();
                return RedirectToAction("Pagar", "MercadoPago", p);
            }
        }

    }
}