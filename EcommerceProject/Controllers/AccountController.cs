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
            return RedirectToAction("Publicaciones", "Account");
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
        public ActionResult Register(Usuario usuario, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                using (var db = new SQLServerContext())
                {
                    var usuarioAEncontrar = db.Usuarios.SingleOrDefault(u => u.Email == usuario.Email);
                    String tipoDeIdentificacion = form["TipoDeIdentificacion"];

                    // VALIDA QUE EL E-MAIL INGRESADO NO EXISTA EN LA BASE DE DATOS
                    if (usuarioAEncontrar != null)
                    {
                        ViewBag.Message = "El E-Mail que quiere registrar ya existe.";
                        return View();
                    }

                    // VALIDA QUE SI SE COMPLETA EL TIPO O NÚMERO DE IDENTIFICACIÓN, ESTÉ EL CAMPO RESTANTE COMPLETO TAMBIÉN
                    if ((tipoDeIdentificacion == null && usuario.Documento != null) || (tipoDeIdentificacion != null && usuario.Documento == null))
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

        public ActionResult Publicaciones()
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
                return RedirectToAction("Publicaciones");
            }

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
                var publicacion = db.Publicaciones.Where(p => p.Usuario.Id == usuarioId && p.Id == publicacionId).FirstOrDefault();

                if (publicacion == null || publicacion.Estado == "Desactivada")
                {
                    return View("Error");
                }

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
                int usuarioId = Int32.Parse(Session["UserId"].ToString());

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
                        String fotoExtension = foto.FileName.Substring(foto.FileName.LastIndexOf('.') + 1).ToLower();

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
                            publi.Foto = fotoFinalPath;
                        }
                        else
                        {
                            ModelState.AddModelError("Foto", "Tipo de archivo no válido. Extensiones permitidas: jpg, jpeg o png.");
                            return View("../Publication/EditPublication", publi);
                        }
                    }

                    if (cv != null)
                    {
                        String CvExtension = cv.FileName.Substring(cv.FileName.LastIndexOf('.') + 1).ToLower();

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
                            publi.CV = cvFinalPath;
                        }
                        else
                        {
                            ModelState.AddModelError("CV", "Tipo de archivo no válido. Extensión permitida: pdf.");
                            return View("../Publication/EditPublication", publi);
                        }
                    }

                    db.SaveChanges();

                    return RedirectToAction("Publicaciones");
                }
                else
                {
                    return View();
                }
            }
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

        public ActionResult EditUser()
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
        public ActionResult EditUser(Usuario usuario, FormCollection form)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (ModelState.IsValid)
            {
                String tipoDeIdentificacion = form["TipoDeIdentificacion"];
                int usuarioId = Int32.Parse(Session["UserId"].ToString());

                using (var db = new SQLServerContext())
                {
                    // E-MAIL DE LA BASE 
                    var usuarioAEditar = db.Usuarios.SingleOrDefault(u => u.Id == usuarioId);

                    // E-MAILS A COMPARAR
                    var emailAbuscar = db.Usuarios.Where(u => u.Email == usuario.Email).FirstOrDefault();

                    // VALIDA QUE EL E-MAIL INGRESADO NO EXISTA EN LA BASE DE DATOS
                    if (emailAbuscar != null && usuario.Email != usuarioAEditar.Email)
                    {
                        ViewBag.Message = "El E-Mail ingresado ya existe.";
                        return View(usuario);
                    }

                    // VALIDA QUE SI SE COMPLETA EL TIPO O NÚMERO DE IDENTIFICACIÓN, ESTÉ EL CAMPO RESTANTE COMPLETO TAMBIÉN
                    if ((tipoDeIdentificacion == null && usuario.Documento != null) || (tipoDeIdentificacion != null && usuario.Documento == null))
                    {
                        ViewBag.Message = "Debe completar el tipo y número de identificación.";
                        return View(usuario);
                    }

                    // VALIDA QUE EL USUARIO SEA MAYOR DE EDAD
                    if (usuario.FechaDeNacimiento.Value.AddYears(18) > DateTime.Today)
                    {
                        ViewBag.Message = "La edad ingresada debe ser mayor o igual a 18 años.";
                        return View(usuario);
                    }

                    usuarioAEditar.Nombre = usuario.Nombre;
                    usuarioAEditar.Apellido = usuario.Apellido;
                    usuarioAEditar.TipoDocumento = tipoDeIdentificacion;
                    usuarioAEditar.Documento = usuario.Documento;
                    usuarioAEditar.FechaDeNacimiento = usuario.FechaDeNacimiento;
                    usuarioAEditar.Telefono = usuario.Telefono;
                    if (usuario.Email != usuarioAEditar.Email)
                    {
                        usuarioAEditar.Email = usuario.Email;
                    }
                    usuarioAEditar.ConfirmPassword = usuarioAEditar.Password;

                    db.SaveChanges();

                    ModelState.Clear();

                    ViewBag.Message = "Usted ha modificado sus datos con éxito.";

                    return RedirectToAction("UserInfo");
                }
            }
            return View(usuario);
        }

        // SERVICIOS CONTRATADOS
        public ActionResult GetContratacionesRealizadas()
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
                var contrataciones = db.Contrataciones
                    .Include("Publicacion")
                    .Include("Usuario")
                    .Include("Publicacion.Usuario")
                    .Include("FechaContratacion")
                    .Include("Publicacion.PublicacionCalificaciones")
                    .Include("Pago")
                    .Where(c => c.Usuario.Id == usuarioId && (c.Estado == "Contratada" || c.Estado == "Pendiente" || c.Estado == "Cancelada" || c.Estado == "Finalizada"))
                    .OrderByDescending(c => c.Id)
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

            if (Session["isAdmin"] != null)
            {
                return View("NotAuthorized");
            }

            int usuarioId = Int32.Parse(Session["UserId"].ToString());

            using (var db = new SQLServerContext())
            {
                var contrataciones = db.Contrataciones
                    .Include("Publicacion")
                    .Include("Usuario")
                    .Include("Usuario.UsuarioCalificacion")
                    .Include("FechaContratacion")
                    .Include("Pago")
                    .Where(c => c.Publicacion.Usuario.Id == usuarioId && (c.Estado == "Contratada" || c.Estado == "Pendiente" || c.Estado == "Cancelada" || c.Estado == "Finalizada"))
                    .OrderByDescending(c => c.Id)
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

            if (Session["isAdmin"] != null)
            {
                return View("NotAuthorized");
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


                    var contratacion = db.Contrataciones
                        .Include("FechaContratacion")
                        .SingleOrDefault(c => c.Id == contratacionId);
                    foreach (FechaContratacion fecha in contratacion.FechaContratacion)
                    {
                        if (!FechaMayorA96Horas(fecha))
                        {
                            return Json("Error");
                        }
                    }

                    contratacion.Estado = "Cancelada";

                    db.SaveChanges();

                    return Json("cancelada", JsonRequestBehavior.AllowGet);
                }
            }
        }


        public bool FechaMayorA96Horas(FechaContratacion fecha) {
            DateTime diaDeHoy = DateTime.Today;
            DateTime fechaContratacion = fecha.Fecha.Date;
            TimeSpan diferencia = fechaContratacion - diaDeHoy;
            int diasDeDiferencia = diferencia.Days;
            if (diasDeDiferencia > 4)
            {
                return true;
            }
            else {
                return false;
            }
        }
    }
}