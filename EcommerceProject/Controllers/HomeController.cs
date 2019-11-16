using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new SQLServerContext())
            {
                var publicacionesPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado != "Desactivada" && p.Promocionada == true)
                    .OrderByDescending(p => p.FechaDeModificacion).ToList();
                var publicacionesNoPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado != "Desactivada" && p.Promocionada == false)
                    .OrderByDescending(p => p.FechaDeModificacion).ToList();
                var publicaciones = publicacionesPromocionadas.Concat(publicacionesNoPromocionadas).ToList();                
               
                return View(publicaciones);
                
            }
        }

        public ActionResult VerDetalle(int id)
        {
            using (var db = new SQLServerContext())
            {
                var publicacion = db.Publicaciones
                    .Include("Usuario")
                    .Where(p => p.Id == id).FirstOrDefault();
                if (publicacion != null)
                {
                    return View(publicacion);
                }
                else
                {
                    return View("Error");
                }
            }
        }

        public JsonResult DameDiasNoDisponiblesPubli(int publicacionId) {
            using (var db = new SQLServerContext())
            {
                var publicacion = db.Publicaciones
                    .Where(p => p.Id == publicacionId).FirstOrDefault();
                if (publicacion != null)
                {
                    var disponibilidad = publicacion.Disponibilidad;
                    var NoDisponibilidad = string.Join(",", DameDiasNoDisponibles(disponibilidad));
                    return Json(NoDisponibilidad, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }
        public string[] DameDiasNoDisponibles(string DiasDisponibles) {
            string[] semana = new string[] {
                "0",
                "1",
                "2",
                "3",
                "4",
                "5",
                "6"
            };
            var semanaList = new List<string>(semana);
            List<String> diasParseados  = DiasDisponibles.Split(',').ToList();
            foreach (var dia in diasParseados) {
                semanaList.Remove(dia);
            }
            return semanaList.ToArray();
        }


    }
}