using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceProject.Controllers
{
    public class PublicationController : Controller
    {
        // LISTA LAS PUBLICACIONES (NO REQUIERE AUTENTICACIÓN)
        public ActionResult List()
        {
            using (var db = new SQLServerContext())
            {
                var publicaciones = db.Publicaciones.Where(p => p.Visible == true && p.Estado != "Desactivada");

                if (publicaciones.Count() > 0)
                {
                    return View(publicaciones);
                }
                else
                {
                    return View("Error");
                }
            }
        }

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

        [HttpPost]
        public ActionResult SavePublication(Publicacion publicacion, FormCollection form, HttpPostedFileBase foto, HttpPostedFileBase cv)
        {
            if (Session["UserId"] == null)
            {
                return View("NotAuthorized");
            }

            ModelState.Remove("Estado");

            if (ModelState.IsValid)
            {
                int usuarioId = Int32.Parse(Session["UserId"].ToString());

                String pathFoto = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(foto.FileName));
                foto.SaveAs(pathFoto);

                String pathCv = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(cv.FileName));
                cv.SaveAs(pathCv);

                using (var db = new SQLServerContext())
                {
                    var disponibilidad = form["Disponibilidad[]"];
                    Usuario usuario = db.Usuarios.Find(usuarioId);

                    publicacion.Disponibilidad = disponibilidad;
                    publicacion.Foto = pathFoto;
                    publicacion.CV = pathCv;
                    publicacion.FechaDePublicacion = Convert.ToDateTime(DateTime.Now);
                    publicacion.FechaDeModificacion = Convert.ToDateTime(DateTime.Now);
                    publicacion.Estado = "Pendiente";
                    publicacion.Usuario = usuario;

                    db.Publicaciones.Add(publicacion);
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

        public ActionResult DisablePublication(int publicacionId)
        {
            if (Session["UserId"] == null)
            {
                return View("NotAuthorized");
            }

            using (var db = new SQLServerContext())
            {
                Publicacion publicacion = db.Publicaciones.Find(publicacionId);
                publicacion.Estado = "Desactivada";
                publicacion.FechaDeModificacion = Convert.ToDateTime(DateTime.Now);

                db.SaveChanges();

                return RedirectToAction("UserInfo", "Account");
            }
        }

        public ActionResult PromocionarPublicacion(int publicacionId)
        {
            if (Session["UserId"] == null)
            {
                return View("NotAuthorized");
            }

            using (var db = new SQLServerContext())
            {
                Publicacion publicacion = db.Publicaciones.Find(publicacionId);

                return RedirectToAction("Pagar", "MercadoPago", publicacion);
            }
        }

        [HttpPost]
        public ActionResult CrearContratacion(String[] diasSeleccionados, int usuarioId, int publicacionId)
        {
            using (var db = new SQLServerContext())
            {
                var usuario = db.Usuarios.SingleOrDefault(u => u.Id == usuarioId);
                var publicacion = db.Publicaciones.SingleOrDefault(p => p.Id == publicacionId);

                if (publicacion.Usuario.Id == usuarioId)
                {
                    return Json("NOTOK", JsonRequestBehavior.AllowGet);
                }

                Contratacion contratacion = new Contratacion
                {
                    Estado = "Pendiente",
                    Publicacion = publicacion,
                    Usuario = usuario
                };

                db.Contrataciones.Add(contratacion);

                // 1 CONTRATACIÓN --> N FECHAS
                foreach (var diaSeleccionado in diasSeleccionados)
                {
                    var settings = new JsonSerializerSettings
                    {
                        DateFormatString = "yyyy-MM-ddTH:mm:ss.fffK",
                        DateTimeZoneHandling = DateTimeZoneHandling.Utc
                    };

                    var dia = JsonConvert.DeserializeObject(diaSeleccionado, settings);
                    DateTime fecha = Convert.ToDateTime(dia).Date;

                    FechaContratacion fechaContratacion = new FechaContratacion
                    {
                        Contratacion = contratacion,
                        Fecha = fecha
                    };

                    db.FechasXContratacion.Add(fechaContratacion);
                }

                db.SaveChanges();

                return Json(contratacion.Id, JsonRequestBehavior.AllowGet);
            }
        }
    }
}