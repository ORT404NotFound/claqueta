using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace EcommerceProject.Controllers
{

    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("UserInfo", "Account");
        }
        /// register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Usuario user)
        {
            if (ModelState.IsValid)
            {
                using (var db = new SQLServerContext())
                {
                    var userToFind = db.Usuarios.SingleOrDefault(u => u.Email == user.Email);

                    if (userToFind != null)
                    {
                        ViewBag.Message = "El email que se quiere registrar ya existe";
                        return View();
                    }

                    // VALIDA QUE EL USUARIO SEA MAYOR DE EDAD

                    if (user.FechaDeNacimiento.Value.AddYears(18) > DateTime.Today)
                    {
                        ViewBag.Message = "El usuario debe tener más de 18 años";
                        return View();
                    }

                    user.Activo = true;
                    db.Usuarios.Add(user);
                    Rol r = db.Roles.SingleOrDefault(role => role.Nombre == "USER");
                    user.Roles.Add(r);
                    db.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = "Usted se ha registrado correctamente";
                    return View();
                }
            }
            return View();
        }

        /// login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Usuario user)
        {
            using (var db = new SQLServerContext())
            {
                var rUser = db.Roles.SingleOrDefault(role => role.Nombre == "USER");
                var rAdmin = db.Roles.SingleOrDefault(role => role.Nombre == "ADMIN");
                var userToFind = db.Usuarios.FirstOrDefault(u => u.Email == user.Email
                                                   && u.Password == user.Password);
                if (userToFind != null)
                {
                    if (userToFind.Roles.Contains(rAdmin))
                    {
                        Session["UserId"] = userToFind.Id;
                        Session["Email"] = user.Email;
                        Session["isAdmin"] = "true";
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (userToFind.Roles.Contains(rUser))
                    {
                        Session["UserId"] = userToFind.Id;
                        Session["Email"] = user.Email;
                        return RedirectToAction("LoggedIn");
                    }
                    else
                    {
                        return View("../Shared/NotAuthorized");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email o contraseña incorrecta");
                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult LogOut()
        {
            Session["UserId"] = null;
            Session["Email"] = null;
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
            int userId = Int32.Parse(Session["UserId"].ToString());
            using (var db = new SQLServerContext())
            {
                var publications = db.Publicaciones.Where(
                        p => p.Estado != "Desactivada" &&
                        p.Usuario.Id == userId
                    ).ToList();

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
        // Obtiene la publicacion a buscar usuario logueado
        public ActionResult EditPublication(int publicationId)
        {
            if (publicationId == 0)
            {
                return RedirectToAction("UserInfo");
            }
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }
            int userId = Int32.Parse(Session["UserId"].ToString());
            using (var db = new SQLServerContext())
            {
                var publi = db
                    .Publicaciones
                    .Where(p => p.Usuario.Id == userId && p.Id == publicationId)
                    .FirstOrDefault();
                if (publi != null)
                {
                    return View("../Publication/EditPublication", publi);
                }
                else
                {
                    return View("Error");
                }
            }
        }

        [HttpPost]
        public ActionResult EditPublication(Publicacion publication, FormCollection form)
        {
            using (var db = new SQLServerContext())
            {
                var dis = form["Disponibilidad[]"];
                var publi = db.Publicaciones.SingleOrDefault(p => p.Id == publication.Id);
                if (publi != null)
                {
                    publi.Promocionada = publication.Promocionada;
                    publi.Titulo = publication.Titulo;
                    publi.Descripcion = publication.Descripcion;
                    publi.CV = publication.CV;
                    publi.Categoria = publication.Categoria;
                    publi.Ubicacion = publication.Ubicacion;
                    publi.Precio = publication.Precio;
                    publi.Reel = publication.Reel;
                    publi.Foto = publication.Foto;
                    publi.Referencias = publication.Referencias;
                    publi.Estado = "Pendiente";
                    publi.Visible = false;


                    publi.Disponibilidad = dis;

                    db.SaveChanges();
                    return RedirectToAction("UserInfo");
                }
                else
                {
                    if (dis == null)
                    {
                        ModelState.AddModelError("Disponibilidad", "Debe seleccionar al menos un dia");
                    }
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
            int userId = Int32.Parse(Session["UserId"].ToString());
            using (var db = new SQLServerContext())
            {
                var user = db
                    .Usuarios.Where(u => u.Id == userId)
                    .FirstOrDefault();
                if (user != null)
                {
                    return View(user);
                }
                else
                {
                    return View("Error");
                }
            }
        }
        [HttpPost]
        public ActionResult EditUser(Usuario user)
        {
            using (var db = new SQLServerContext())
            {
                var userToUpdate = db.Usuarios.SingleOrDefault(u => u.Id == user.Id);
                if (userToUpdate != null)
                {
                    userToUpdate.Nombre = user.Nombre;
                    userToUpdate.Apellido = user.Apellido;
                    db.SaveChanges();
                    return RedirectToAction("UserInfo");
                }
                else
                {
                    return View("Error");
                }
            }
        }

        //contrataciones q hizo
        public ActionResult GetContratacionesRealizadas()
        {

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }
            int userId = Int32.Parse(Session["UserId"].ToString());
            using (var db = new SQLServerContext())
            {
                var contataciones = db.Contrataciones
                    .Include("Publicacion")
                    .Where(c => c.Usuario.Id == userId && (c.Estado == "Contratada" || c.Estado == "Pendiente"))
                    .ToList();
                return View(contataciones);
            }
        }

        // al que lo contratan
        public ActionResult GetContratacionesDelUsuario()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }
            int userId = Int32.Parse(Session["UserId"].ToString());
            using (var db = new SQLServerContext())
            {
                var contataciones = db.Contrataciones
                    .Include("Publicacion")
                    .Where(c => c.Publicacion.Usuario.Id == userId && (c.Estado == "Contratada" || c.Estado == "Pendiente"))
                    .ToList();
                return View(contataciones);
            }
        }


        public ActionResult GetConsultasDelUsuario()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }
            int userId = Int32.Parse(Session["UserId"].ToString());
            using (var db = new SQLServerContext())
            {
                var consultas = db.Consultas
                    .Include("Publicacion")
                    .Include("Usuario")
                    .Where(c => c.Publicacion.Usuario.Id == userId && c.Respuesta == null && c.Visible == true)
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
                int userId = Int32.Parse(Session["UserId"].ToString());
                var publiId = Int32.Parse(form["publiId"]);
                using (var db = new SQLServerContext())
                {
                    var publi = db.Publicaciones.SingleOrDefault(p => p.Id == publiId);
                    var user = db.Usuarios.SingleOrDefault(u => u.Id == userId);
                    consulta.Usuario = user;
                    consulta.Publicacion = publi;
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
                int consId = Int32.Parse(form["id"].ToString());
                var respuesta = form["texto"];
                using (var db = new SQLServerContext())
                {
                    var consulta = db.Consultas.SingleOrDefault(c => c.Id == consId);
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
                int consId = Int32.Parse(form["id"].ToString());
                using (var db = new SQLServerContext())
                {
                    var consulta = db.Consultas.SingleOrDefault(c => c.Id == consId);
                    consulta.Visible = false;
                    db.SaveChanges();
                    return Json("eliminada", JsonRequestBehavior.AllowGet);
                }

            }
        }


    }
}