using EcommerceProject.Models.EcommerceProject.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EcommerceProject.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (Session["isAdmin"] == null)
            {
                return View("NotAuthorized");
            }

            int usuarioId = Int32.Parse(Session["UserId"].ToString());

            using (var db = new SQLServerContext())
            {
                var publicaciones = db.Publicaciones.Where(p => p.Estado != "Desactivada" && p.Estado == "Pendiente").ToList();

                if (publicaciones != null)
                {
                    return View(publicaciones);
                }
                else
                {
                    return View("Error");
                }
            }
        }

        public ActionResult AprobarPublicacion(int publicacionId)
        {
            if (publicacionId == 0)
            {
                return RedirectToAction("Index");
            }

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (Session["isAdmin"] == null)
            {
                return View("NotAuthorized");
            }

            int usuarioId = Int32.Parse(Session["UserId"].ToString());

            using (var db = new SQLServerContext())
            {
                var publicacion = db.Publicaciones.Where(p => p.Id == publicacionId).FirstOrDefault();

                if (publicacion != null)
                {
                    publicacion.Estado = "Aprobada";
                    publicacion.Visible = true;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error");
                }
            }
        }

        public ActionResult RechazarPublicacion(int publicacionId)
        {
            if (publicacionId == 0)
            {
                return RedirectToAction("Index");
            }

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (Session["isAdmin"] == null)
            {
                return View("NotAuthorized");
            }

            int usuarioId = Int32.Parse(Session["UserId"].ToString());

            using (var db = new SQLServerContext())
            {
                var publicacion = db.Publicaciones.Where(p => p.Id == publicacionId).FirstOrDefault();

                if (publicacion != null)
                {
                    publicacion.Estado = "Rechazada";
                    publicacion.Visible = false;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error");
                }
            }
        }
    }
}