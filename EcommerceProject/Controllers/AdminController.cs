using EcommerceProject.Models.EcommerceProject.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EcommerceProject.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["isAdmin"] == null)
            {
                return View("../Shared/NotAuthorized");
            }

            int userId = Int32.Parse(Session["UserId"].ToString());

            using (var db = new SQLServerContext())
            {
                var publications = db.Publicaciones.Where(p => p.Estado != "Desactivada" && p.Estado == "Pendiente").ToList();

                if (publications != null)
                {
                    return View(publications);
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
                return View("../Shared/NotAuthorized");
            }

            int userId = Int32.Parse(Session["UserId"].ToString());

            using (var db = new SQLServerContext())
            {
                var publi = db
                    .Publicaciones
                    .Where(p => p.Id == publicacionId)
                    .FirstOrDefault();

                if (publi != null)
                {
                    publi.Estado = "Aprobada";
                    publi.Visible = true;
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
                return View("../Shared/NotAuthorized");
            }

            int userId = Int32.Parse(Session["UserId"].ToString());

            using (var db = new SQLServerContext())
            {
                var publi = db
                    .Publicaciones
                    .Where(p => p.Id == publicacionId)
                    .FirstOrDefault();

                if (publi != null)
                {
                    publi.Estado = "Rechazada";
                    publi.Visible = false;
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