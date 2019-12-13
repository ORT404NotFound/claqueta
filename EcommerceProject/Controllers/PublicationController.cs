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

            ModelState.Remove("Categoria");
            ModelState.Remove("Estado");

            if (ModelState.IsValid)
            {
                int usuarioId = Int32.Parse(Session["UserId"].ToString());
                String fotoExtension = foto.FileName.Substring(foto.FileName.LastIndexOf('.') + 1).ToLower();
                String CvExtension = cv.FileName.Substring(cv.FileName.LastIndexOf('.') + 1).ToLower();

                if (fotoExtension == "jpg" || fotoExtension == "jpeg" || fotoExtension == "png")
                {
                    var fotoFileName = Path.GetFileName(foto.FileName);
                    var fotoGuid = Guid.NewGuid().ToString();
                    var fotoPath = Path.Combine(Server.MapPath("~/UploadedFiles"), usuarioId + "_" + fotoGuid + "_" + fotoFileName);
                    foto.SaveAs(fotoPath);
                    String fotoFl = fotoPath.Substring(fotoPath.LastIndexOf("\\"));
                    String[] fotoSplit = fotoFl.Split('\\');
                    String fotoNewPath = fotoSplit[1];
                    String fotoFinalPath = "/UploadedFiles/" + fotoNewPath;
                    publicacion.Foto = fotoFinalPath;
                }
                else
                {
                    // AGREGAR MENSAJE DE ERROR
                    // Tipo de archivo no válido. Extensiones permitidas: jpg, jpeg o png.
                }

                if (CvExtension == "pdf")
                {
                    var cvFileName = Path.GetFileName(cv.FileName);
                    var cvGuid = Guid.NewGuid().ToString();
                    var cvPath = Path.Combine(Server.MapPath("~/UploadedFiles"), usuarioId + "_" + cvGuid + "_" + cvFileName);
                    cv.SaveAs(cvPath);
                    String cvFl = cvPath.Substring(cvPath.LastIndexOf("\\"));
                    String[] cvSplit = cvFl.Split('\\');
                    String cvNewPath = cvSplit[1];
                    String cvFinalPath = "/UploadedFiles/" + cvNewPath;
                    publicacion.CV = cvFinalPath;
                }
                else
                {
                    // AGREGAR MENSAJE DE ERROR
                    // Tipo de archivo no válido. Extensión permitida: pdf.
                }

                using (var db = new SQLServerContext())
                {
                    var categoria = form["categoria"];
                    var disponibilidad = form["Disponibilidad[]"];
                    Usuario usuario = db.Usuarios.Find(usuarioId);

                    publicacion.Categoria_Id = Convert.ToInt32(categoria);
                    publicacion.Disponibilidad = disponibilidad;
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

                return RedirectToAction("Publicaciones", "Account");
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
                DateTime fecha;
                foreach (var diaSeleccionado in diasSeleccionados)
                {
                    fecha = TransformarFecha(diaSeleccionado);
                    if (EstaDisponibleLaFecha(fecha, publicacion))
                    {
                        FechaContratacion fechaContratacion = new FechaContratacion
                        {
                            Contratacion = contratacion,
                            Fecha = fecha
                        };
                        db.FechasXContratacion.Add(fechaContratacion);
                    }
                }
                db.SaveChanges();

                return Json(contratacion.Id, JsonRequestBehavior.AllowGet);
            }
        }

        public DateTime TransformarFecha(String fecha)
        {
            var settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-ddTH:mm:ss.fffK",
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            var dia = JsonConvert.DeserializeObject(fecha, settings);
            DateTime fechaDate = Convert.ToDateTime(dia).Date;
            return fechaDate;
        }

        public bool EstaDisponibleLaFecha(DateTime fechaEnParticular, Publicacion publicacion)
        {
            using (var db = new SQLServerContext())
            {
                var HayContatacionesEnEsaFecha = db.FechasXContratacion.SingleOrDefault(f => f.Fecha == fechaEnParticular && f.Contratacion.Publicacion_Id == publicacion.Id);
                if (HayContatacionesEnEsaFecha != null)
                {
                    return false;
                }
                DayOfWeek diaDeLaSemana = fechaEnParticular.DayOfWeek;
                int numeroDeLaSemana = (int)diaDeLaSemana;
                var diasDisp = publicacion.Disponibilidad.Split(',').Select(Int32.Parse).ToList();
                if (!diasDisp.Contains(numeroDeLaSemana))
                {
                    return false;
                }
                return true;
            }
        }

        public ActionResult FinalizarContratacion(int contratacionId) 
        {
            if (Session["UserId"] == null)
            {
                return Json("NotAuthorized");
            }
            using (var db = new SQLServerContext())
            {
                var contratacionABuscar = db.Contrataciones.Find(contratacionId);
                if (contratacionABuscar != null)
                {
                    contratacionABuscar.Estado = "Finalizada";
                    db.SaveChanges();
                    return Json("OK");
                }
                else {
                    return Json("Error");
                }
            }

        }
    }
}