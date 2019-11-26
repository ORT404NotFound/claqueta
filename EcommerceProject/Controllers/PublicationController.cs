using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceProject.Controllers
{

    public class PublicationController : Controller
    {
        //muestra el listado de publicaciones (no requiere auth)
        public ActionResult List()
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

        //muestra vista para guardar publicacion
        public ActionResult SavePublication()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return View("NotAuthorized");
            }
        }

        // PARA GUARDAR UNA PUBLICACIÓN, NECESITA ESTAR AUTENTICADO COMO USUARIO
        [HttpPost]
        public ActionResult SavePublication(Publicacion publication, FormCollection form, HttpPostedFileBase foto, HttpPostedFileBase cv)
        {
            if (Session["UserId"] == null)
            {
                return View("NotAuthorized");
            }

            if (ModelState.IsValid)
            {
                int userId = Int32.Parse(Session["UserId"].ToString());

                string pathFoto = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(foto.FileName));
                foto.SaveAs(pathFoto);

                string pathCv = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(cv.FileName));
                cv.SaveAs(pathCv);

                using (var db = new SQLServerContext())
                {
                    Usuario u = db.Usuarios.Find(userId);
                    publication.Usuario = u;
                    publication.Estado = "Pendiente";
                    publication.FechaDeModificacion = Convert.ToDateTime(DateTime.Now);
                    publication.FechaDePublicacion = Convert.ToDateTime(DateTime.Now);
                    var dis = form["Disponibilidad[]"];
                    publication.Disponibilidad = dis;
                    publication.Foto = pathFoto;
                    publication.CV = pathCv;
                    db.Publicaciones.Add(publication);
                    db.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = "La publicacion fue guardada exitosamente.";
                    return View();
                }
            }
            else
            {
                if (form["Disponibilidad[]"] == null)
                {
                    ModelState.AddModelError("Disponibilidad", "Debe seleccionar al menos un día de la semana.");
                }
                return View();
            }
        }

        public ActionResult DisablePublication(int idPublication)
        {
            if (Session["UserId"] == null)
            {
                return View("NotAuthorized");
            }
            using (var db = new SQLServerContext())
            {
                Publicacion p = db.Publicaciones.Find(idPublication);
                p.Estado = "Desactivada";
                p.FechaDeModificacion = Convert.ToDateTime(DateTime.Now);
                db.SaveChanges();
                return RedirectToAction("UserInfo", "Account");
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
                p.Promocionada = true;
                p.FechaDeModificacion = Convert.ToDateTime(DateTime.Now);
                db.SaveChanges();
                return RedirectToAction("Pagar", "MercadoPago", p);
            }
        }

        [HttpPost]
        public ActionResult CrearContratacion(String[] diasSeleccionados, int usuarioId, int publicacionId)
        {
            var result = string.Join(",", diasSeleccionados);

            using (var db = new SQLServerContext())
            {
                var userToFind = db.Usuarios.SingleOrDefault(u => u.Id == usuarioId);
                var publicacion = db.Publicaciones.SingleOrDefault(p => p.Id == publicacionId);

                if (publicacion.Usuario.Id == usuarioId)
                {
                    return Json("NOTOK", JsonRequestBehavior.AllowGet);
                }

                Contratacion contratacion = new Contratacion
                {
                    Estado = "Pendiente",
                    Fechas = result,
                    Publicacion = publicacion,
                    Usuario = userToFind
                };
                db.Contrataciones.Add(contratacion);
                db.SaveChanges();

                return Json(contratacion.Id, JsonRequestBehavior.AllowGet);
            }
        }
    }
}