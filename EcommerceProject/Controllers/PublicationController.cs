using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceProject.Controllers
{
    public class PublicationController : Controller
    {
        public ActionResult List()
        {

            using (var db = new SQLServerContext())
            {
                var publis = db.Publications.Where(p => p.Visible == true);
                if (publis.Count() > 0)
                {
                    return View(publis);
                }
                else {
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
            else {
                return View("NotAuthorized");
            }
        }
        
        // para guardar una publicacion, necesita auth de user
        [HttpPost]
        public ActionResult SavePublication(Publication publication)
        {
            if (Session["UserId"] == null) {
                return View("NotAuthorized");
            }
            if (ModelState.IsValid) {
                int userId;
                try
                {
                    userId = Int32.Parse(Session["UserId"].ToString());
                }
                catch (FormatException e)
                {
                    throw e;
                }
                using (var db = new SQLServerContext())
                {
                    User u = db.Users.Find(userId);
                    publication.User = u;
                    db.Publications.Add(publication);
                    db.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = "La publicacion fue guardada exitosamente";
                    return View();

                }
            }
            return View();
        }


        // una publicacion en particular para mostrar en el home, tipo la de detalles 
        public ActionResult GetHomePublication(int id)
        {  
            using (var db = new SQLServerContext())
            {
                var publi = db.Publications.FirstOrDefault(p => p.Id == id);
                if (publi != null)
                {
                    return View("HomePublication",publi);
                }
               
            }
            return View("Error");
        }

    }
}