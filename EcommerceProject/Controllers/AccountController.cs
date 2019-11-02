using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EcommerceProject.Controllers
{

    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        /// register
        public ActionResult Register() {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Usuario user) {
            if (ModelState.IsValid)
            {
                using (var db = new SQLServerContext())
                {
                    var userToFind = db.Usuarios.SingleOrDefault(u => u.Email == user.Email);

                    if (userToFind != null) {
                        ViewBag.Message = "El email que se quiere registrar ya existe";
                        return View();
                    }
                    user.Activo = 1;
                    db.Usuarios.Add(user);
                    Rol r = db.Roles.SingleOrDefault(role => role.Nombre == "USER");
                    user.Roles.Add(r);
                    db.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = user.Id + "registrado correctamente";
                    return View();
                }
            }
            return View();
        }

        /// login
        public ActionResult Login() {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Usuario user) {
            using (var db = new SQLServerContext())
            {

                var r = db.Roles.SingleOrDefault(role => role.Nombre == "USER");
                var userToFind = db.Usuarios.SingleOrDefault(u => u.Email == user.Email
                                                   && u.Password == user.Password);
              
                if (userToFind != null)
                {
                    if (!userToFind.Roles.Contains(r))
                    {
                        return View("../Shared/NotAuthorized");
                    }
                    Session["UserId"] = userToFind.Id;
                    Session["Email"] = user.Email;
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "Email o contraseña incorrecta");
                }
            }
            return View();
        }

        public ActionResult LoggedIn() {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else {
                return RedirectToAction("Login");
            }
        }

        public ActionResult LogOut() {
            Session["UserId"] = null;
            Session["Email"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult UserInfo() {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }
            int userId = Int32.Parse(Session["UserId"].ToString());
            using (var db = new SQLServerContext()) {
                var publications = db.Publicaciones.Where(
                        p => p.Estado != "Desactivada" &&
                        p.Usuario.Id == userId
                    ).ToList();
                       
                if (publications != null)
                {
                    return View(publications);
                }
                else {
                    return View("Error");
                }
            }
        }
        // Obtiene la publicacion a buscar usuario logueado
        public ActionResult EditPublication(int publicationId) {
            if (publicationId == 0) {
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
                    return View("../Publication/EditPublication",publi);
                }
                else
                {
                    return View("Error");
                }
            }
        }

        [HttpPost]
        public ActionResult EditPublication(Publicacion publication) {
            using (var db = new SQLServerContext())
            {
                var publi = db.Publicaciones.SingleOrDefault(p => p.Id == publication.Id);
                if (publi != null)
                {
                    publi.Promocionada = publication.Promocionada;
                    publi.Descripcion = publication.Descripcion;
                    publi.CV = publication.CV;
                    publi.Categoria = publication.Categoria;
                    publi.Ubicacion = publication.Ubicacion;
                    publi.Precio = publication.Precio;
                    publi.Reel = publication.Reel;

                    publi.Garantia = publication.Garantia;
                    publi.Visible = publication.Visible;
                    publi.Foto = publication.Foto;
                    publi.Referencias = publication.Referencias;

                    db.SaveChanges();
                    return RedirectToAction("UserInfo");
                }
                else {
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
        public ActionResult EditUser (Usuario user)
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
    }
}