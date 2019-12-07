using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceProject.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("UserInfo", "Account");
        }

        public ActionResult Calificaciones()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }

            if (Session["isAdmin"] != null)
            {
                return View("NotAuthorized");
            }

            return View();
        }

        public ActionResult Register()
        {
            if (Session["UserId"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Register(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                using (var db = new SQLServerContext())
                {
                    var usuarioAEncontrar = db.Usuarios.SingleOrDefault(u => u.Email == usuario.Email);

                    // VALIDA QUE EL E-MAIL INGRESADO NO EXISTA EN LA BASE DE DATOS
                    if (usuarioAEncontrar != null)
                    {
                        ViewBag.Message = "El E-Mail que quiere registrar ya existe.";
                        return View();
                    }

                    // VALIDA QUE SI SE COMPLETA EL TIPO O NÚMERO DE IDENTIFICACIÓN, ESTÉ EL CAMPO RESTANTE COMPLETO TAMBIÉN
                    if ((usuario.TipoDocumento == null && usuario.Documento != null) || (usuario.TipoDocumento != null && usuario.Documento == null))
                    {
                        ViewBag.Message = "Debe completar el tipo y número de identificación.";
                        return View();
                    }

                    // VALIDA QUE EL USUARIO SEA MAYOR DE EDAD
                    if (usuario.FechaDeNacimiento.Value.AddYears(18) > DateTime.Today)
                    {
                        ViewBag.Message = "Debe tener más de 18 años para poder registrarse en la plataforma.";
                        return View();
                    }

                    usuario.Activo = true;
                    db.Usuarios.Add(usuario);

                    Rol rol = db.Roles.SingleOrDefault(r => r.Nombre == "USER");
                    usuario.Roles.Add(rol);

                    db.SaveChanges();

                    ModelState.Clear();

                    ViewBag.Message = "Usted se ha registrado correctamente.";

                    return View();
                }
            }
            return View();
        }

        public ActionResult Login()
        {
            if (Session["UserId"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Login(Usuario usuario)
        {
            using (var db = new SQLServerContext())
            {
                var rUsuario = db.Roles.SingleOrDefault(r => r.Nombre == "USER");
                var rAdmin = db.Roles.SingleOrDefault(r => r.Nombre == "ADMIN");
                var usuarioAEncontrar = db.Usuarios.FirstOrDefault(u => u.Email == usuario.Email && u.Password == usuario.Password);

                if (usuarioAEncontrar != null)
                {
                    if (usuarioAEncontrar.Roles.Contains(rUsuario))
                    {
                        Session["UserId"] = usuarioAEncontrar.Id;
                        Session["Email"] = usuario.Email;

                        return RedirectToAction("Index", "Home");
                    }
                    else if (usuarioAEncontrar.Roles.Contains(rAdmin))
                    {
                        Session["UserId"] = usuarioAEncontrar.Id;
                        Session["Email"] = usuario.Email;
                        Session["isAdmin"] = "true";

                        return RedirectToAction("Index", "Admin");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Los datos ingresados son incorrectos.");
                }
            }
            return View();
        }

        public ActionResult LogOut()
        {
            Session["UserId"] = null;
            Session["Email"] = null;
            Session["isAdmin"] = null;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult UserInfo()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }

            if (Session["isAdmin"] != null)
            {
                return View("NotAuthorized");
            }

            int usuarioId = Int32.Parse(Session["UserId"].ToString());

            using (var db = new SQLServerContext())
            {
                var publicaciones = db.Publicaciones.Include("Categoria").Where(p => p.Estado != "Desactivada" && p.Usuario.Id == usuarioId).ToList();

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

        public ActionResult EditPublication(int publicacionId)
        {
            if (publicacionId == 0)
            {
                return RedirectToAction("UserInfo");
            }

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }

            int usuarioId = Int32.Parse(Session["UserId"].ToString());

            using (var db = new SQLServerContext())
            {
                var publicacion = db.Publicaciones.Where(p => p.Usuario.Id == usuarioId && p.Id == publicacionId).FirstOrDefault();

                if (publicacion != null)
                {
                    return View("../Publication/EditPublication", publicacion);
                }
                else
                {
                    return View("Error");
                }
            }
        }

        [HttpPost]
        public ActionResult EditPublication(Publicacion publicacion, FormCollection form, HttpPostedFileBase foto, HttpPostedFileBase cv)
        {
            using (var db = new SQLServerContext())
            {
                var categoria = form["categoria"];
                var disponibilidad = form["Disponibilidad[]"];
                var publi = db.Publicaciones.SingleOrDefault(p => p.Id == publicacion.Id);

                if (disponibilidad == null)
                {
                    ModelState.AddModelError("Disponibilidad", "Debe seleccionar al menos un día de la semana.");
                    return View("../Publication/EditPublication", publi);
                }

                if (publi != null)
                {
                    publi.Categoria_Id = Convert.ToInt32(categoria);
                    publi.Disponibilidad = disponibilidad;
                    publi.Ubicacion = publicacion.Ubicacion;
                    publi.Titulo = publicacion.Titulo;
                    publi.Descripcion = publicacion.Descripcion;
                    publi.Precio = publicacion.Precio;
                    publi.Referencias = publicacion.Referencias;
                    publi.Reel = publicacion.Reel;
                    publi.Visible = false;
                    publi.Estado = "Pendiente";

                    if (foto != null)
                    {
                        String pathFoto = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(foto.FileName));
                        foto.SaveAs(pathFoto);
                        publi.Foto = pathFoto;
                    }

                    if (cv != null)
                    {
                        String pathCv = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(cv.FileName));
                        cv.SaveAs(pathCv);
                        publi.CV = pathCv;
                    }

                    db.SaveChanges();

                    return RedirectToAction("UserInfo");
                }
                else
                {
                    return View();
                }
            }
        }

        public ActionResult EditUser()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }

            int usuarioId = Int32.Parse(Session["UserId"].ToString());

            using (var db = new SQLServerContext())
            {
                var usuario = db.Usuarios.Where(u => u.Id == usuarioId).FirstOrDefault();

                if (usuario != null)
                {
                    return View(usuario);
                }
                else
                {
                    return View("Error");
                }
            }
        }

        [HttpPost]
        public ActionResult EditUser(Usuario usuario)
        {
            using (var db = new SQLServerContext())
            {
                var usuarioActualizar = db.Usuarios.SingleOrDefault(u => u.Id == usuario.Id);

                if (usuarioActualizar != null)
                {
                    usuarioActualizar.Nombre = usuario.Nombre;
                    usuarioActualizar.Apellido = usuario.Apellido;

                    db.SaveChanges();

                    return RedirectToAction("UserInfo");
                }
                else
                {
                    return View("Error");
                }
            }
        }

        // SERVICIOS CONTRATADOS
        public ActionResult GetContratacionesRealizadas()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }

            int usuarioId = Int32.Parse(Session["UserId"].ToString());

            using (var db = new SQLServerContext())
            {
                var contrataciones = db.Contrataciones
                    .Include("Publicacion")
                    .Where(c => c.Usuario.Id == usuarioId && (c.Estado == "Contratada" || c.Estado == "Pendiente" || c.Estado == "Cancelada"))
                    .ToList();

                return View(contrataciones);
            }
        }

        // MIS CONTRATACIONES
        public ActionResult GetContratacionesDelUsuario()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }

            int usuarioId = Int32.Parse(Session["UserId"].ToString());

            using (var db = new SQLServerContext())
            {
                var contrataciones = db.Contrataciones
                    .Include("Publicacion")
                    .Where(c => c.Publicacion.Usuario.Id == usuarioId && (c.Estado == "Contratada" || c.Estado == "Pendiente" || c.Estado == "Cancelada"))
                    .ToList();

                return View(contrataciones);
            }
        }

        public ActionResult GetConsultasDelUsuario()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }

            int usuarioId = Int32.Parse(Session["UserId"].ToString());

            using (var db = new SQLServerContext())
            {
                var consultas = db.Consultas
                    .Include("Publicacion")
                    .Include("Usuario")
                    .Where(c => c.Publicacion.Usuario.Id == usuarioId && c.Respuesta == null && c.Visible == true)
                    .OrderBy(c => c.Id)
                    .ToList();

                return View(consultas);
            }
        }

        public ActionResult CrearConsulta(Consulta consulta, FormCollection form)
        {
            if (Session["UserId"] == null)
            {
                return Json("NotAllowed", JsonRequestBehavior.AllowGet);
            }
            else
            {
                int usuarioId = Int32.Parse(Session["UserId"].ToString());
                var publicacionId = Int32.Parse(form["publiId"]);

                using (var db = new SQLServerContext())
                {
                    var publicacion = db.Publicaciones.SingleOrDefault(p => p.Id == publicacionId);
                    var usuario = db.Usuarios.SingleOrDefault(u => u.Id == usuarioId);

                    consulta.Usuario = usuario;
                    consulta.Publicacion = publicacion;
                    consulta.Visible = true;

                    db.Consultas.Add(consulta);
                    db.SaveChanges();

                    return Json("guardado", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult ResponderConsulta(FormCollection form)
        {
            if (Session["UserId"] == null)
            {
                return Json("NotAllowed", JsonRequestBehavior.AllowGet);
            }
            else
            {
                int consultaId = Int32.Parse(form["id"].ToString());
                var respuesta = form["texto"];

                using (var db = new SQLServerContext())
                {
                    var consulta = db.Consultas.SingleOrDefault(c => c.Id == consultaId);

                    consulta.Respuesta = respuesta;

                    db.SaveChanges();

                    return Json("guardado", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult EliminarConsulta(FormCollection form)
        {
            if (Session["UserId"] == null)
            {
                return Json("NotAllowed", JsonRequestBehavior.AllowGet);
            }
            else
            {
                int consultaId = Int32.Parse(form["id"].ToString());

                using (var db = new SQLServerContext())
                {
                    var consulta = db.Consultas.SingleOrDefault(c => c.Id == consultaId);

                    consulta.Visible = false;

                    db.SaveChanges();

                    return Json("eliminada", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult CancelarContratacion(FormCollection form)
        {
            if (Session["UserId"] == null)
            {
                return Json("NotAllowed", JsonRequestBehavior.AllowGet);
            }
            else
            {
                int contratacionId = Int32.Parse(form["id"].ToString());

                using (var db = new SQLServerContext())
                {
                    var contratacion = db.Contrataciones.SingleOrDefault(c => c.Id == contratacionId);

                    contratacion.Estado = "Cancelada";

                    db.SaveChanges();

                    return Json("cancelada", JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}