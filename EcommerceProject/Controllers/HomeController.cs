using EcommerceProject.Models.EcommerceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (id == 0)
            {
                return View("Index");
            }

            using (var db = new SQLServerContext())
            {
                var publicacion = db.Publicaciones
                    .Include("Usuario")
                    .Include("Consultas")
                    .Include("Consultas.Usuario")
                    .Include("Consultas.Publicacion")
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

        public JsonResult DameDiasNoDisponiblesPubli(int publicacionId)
        {
            using (var db = new SQLServerContext())
            {
                var publicacion = db.Publicaciones.Where(p => p.Id == publicacionId).FirstOrDefault();

                if (publicacion != null)
                {
                    var disponibilidad = publicacion.Disponibilidad;
                    var NoDisponibilidad = String.Join(",", DameDiasNoDisponibles(disponibilidad));
                    return Json(NoDisponibilidad, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public String[] DameDiasNoDisponibles(String DiasDisponibles)
        {
            String[] semana = new String[] {
                "0",
                "1",
                "2",
                "3",
                "4",
                "5",
                "6"
            };
            var semanaList = new List<String>(semana);
            List<String> diasParseados = DiasDisponibles.Split(',').ToList();
            foreach (var dia in diasParseados)
            {
                semanaList.Remove(dia);
            }
            return semanaList.ToArray();
        }

        public ActionResult DameContratacionesDeUnPrestador(int prestadorId)
        {
            using (var db = new SQLServerContext())
            {
                var userToFind = db.Usuarios.SingleOrDefault(u => u.Id == prestadorId);

                if (userToFind == null)
                {
                    return Json("NOTOK", JsonRequestBehavior.AllowGet);
                }

                var contrataciones = db.Contrataciones.Where(c => c.Publicacion.Usuario.Id == userToFind.Id && c.Estado != "Pendiente" && c.Pago != null).ToArray();

                if (contrataciones == null)
                {
                    return Json("NOTOK", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = string.Join(",", contrataciones.Select(w => w.Fechas));
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult BuscarPublicacionesPorCategoria(String categoria)
        {
            using (var db = new SQLServerContext())
            {
                var publicacionesPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado != "Desactivada" && p.Promocionada == true && p.Categoria.ToLower().Contains(categoria.ToLower()))
                    .OrderByDescending(p => p.FechaDeModificacion).ToList();
                var publicacionesNoPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado != "Desactivada" && p.Promocionada == false && p.Categoria.ToLower().Contains(categoria.ToLower()))
                    .OrderByDescending(p => p.FechaDeModificacion).ToList();
                var publicaciones = publicacionesPromocionadas.Concat(publicacionesNoPromocionadas).ToList();
                return View("BuscadorPublicaciones", publicaciones);
            }
        }

        public ActionResult BuscarPublicaciones(String termino)
        {
            using (var db = new SQLServerContext())
            {
                var publicacionesPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado != "Desactivada" && p.Promocionada == true && p.Titulo.ToLower().Contains(termino.ToLower()))
                    .OrderByDescending(p => p.FechaDeModificacion).ToList();
                var publicacionesNoPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado != "Desactivada" && p.Promocionada == false && p.Titulo.ToLower().Contains(termino.ToLower()))
                    .OrderByDescending(p => p.FechaDeModificacion).ToList();
                var publicaciones = publicacionesPromocionadas.Concat(publicacionesNoPromocionadas).ToList();
                return View("BuscadorPublicaciones", publicaciones);
            }
        }
    }
}