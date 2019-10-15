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
        public ActionResult Register(User user) {
            if (ModelState.IsValid)
            {
                using (var db = new SQLServerContext())
                {
                    var userToFind = db.Users.SingleOrDefault(u => u.Email == user.Email);

                    if (userToFind != null) {
                        ViewBag.Message = "El email que se quiere registrar ya existe";
                        return View();
                    }
                    user.Active = 1;
                    db.Users.Add(user);
                    Role r = db.Roles.SingleOrDefault(role => role.Rolename == "USER");
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
        public ActionResult Login(User user) {
            using (var db = new SQLServerContext())
            {

                var r = db.Roles.SingleOrDefault(role => role.Rolename == "USER");
                var userToFind = db.Users.Single(u => u.Email == user.Email
                                                   && u.Password == user.Password);
                if (!userToFind.Roles.Contains(r)) {
                    return View("../Shared/NotAuthorized");
                }

                if (userToFind != null)
                {
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
    }
}