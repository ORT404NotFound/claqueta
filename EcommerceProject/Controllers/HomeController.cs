using EcommerceProject.Models;
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

        public ActionResult VerDetalle(int publicacionId)
        {
            if (publicacionId == 0)
            {
                return View("Index");
            }

            using (var db = new SQLServerContext())
            {
                var publicacion = db.Publicaciones
                    .Include("Usuario")
                    .Include("Categoria")
                    .Include("Consultas")
                    .Include("Consultas.Usuario")
                    .Include("Consultas.Publicacion")
                    .Where(p => p.Id == publicacionId).FirstOrDefault();

                if (publicacion == null || publicacion.Estado == "Pendiente" || publicacion.Estado == "Desactivada")
                {
                    return RedirectToAction("Index");
                }

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
                    var noDisponibilidad = DameDiasNoDisponibles(disponibilidad);

                    return Json(noDisponibilidad.ToArray(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public int[] DameDiasNoDisponibles(String diasDisponibles)
        {
            int[] semana = new int[] { 0, 1, 2, 3, 4, 5, 6 };

            List<int> semanaList = new List<int>(semana);

            List<String> diasParseados = diasDisponibles.Split(',').ToList();

            foreach (var dia in diasParseados)
            {
                semanaList.Remove(Int32.Parse(dia));
            }

            return semanaList.ToArray();
        }

        public ActionResult DameContratacionesDeUnaPublicacion(int publicacionId)
        {
            using (var db = new SQLServerContext())
            {
                var publicacion = db.Publicaciones.SingleOrDefault(p => p.Id == publicacionId);

                if (publicacion == null)
                {
                    return Json("NOTOK", JsonRequestBehavior.AllowGet);
                }

                var contrataciones = db.Contrataciones.Include("FechaContratacion").Where(c => c.FechaContratacion.Any(fc => fc.Reservada == true && fc.Contratacion.Publicacion.Id == publicacion.Id )).ToArray();

                if (contrataciones == null)
                {
                    return Json("NOTOK", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<DateTime> fechas = new List<DateTime>();

                    foreach (var contratacion in contrataciones)
                    {
                        if (contratacion.FechaContratacion.Count > 0)
                        {
                            foreach (var fecha in contratacion.FechaContratacion)
                            {
                                fechas.Add(fecha.Fecha);
                            }
                        }
                    }

                    return Json(fechas.ToArray(), JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult BuscarPublicacionesPorCategoria(int categoriaId)
        {
            using (var db = new SQLServerContext())
            {
                var categoria = db.Categorias.Where(c => c.Id == categoriaId).FirstOrDefault();

                var publicacionesPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado != "Desactivada" && p.Promocionada == true && p.Categoria.Id == categoriaId)
                    .OrderByDescending(p => p.FechaDeModificacion).ToList();
                var publicacionesNoPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado != "Desactivada" && p.Promocionada == false && p.Categoria.Id == categoriaId)
                    .OrderByDescending(p => p.FechaDeModificacion).ToList();
                var publicaciones = publicacionesPromocionadas.Concat(publicacionesNoPromocionadas).ToList();

                ViewBag.CategoriaPublicacion = categoria.Nombre;

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

                ViewBag.Termino = termino;

                return View("BuscadorPublicaciones", publicaciones);
            }
        }

        public ActionResult PerfilUsuario(int usuarioId)
        {
            using (var db = new SQLServerContext())
            {
                var usuario = db.Usuarios
                    .Include("UsuarioCalificacion")
                    .SingleOrDefault(u => u.Id == usuarioId);

                var calificaciones = db.UsuariosXCalificaciones.Where(uc => uc.Usuario.Id == usuario.Id).Select(x => x.Puntaje);
                Double calificacionPromedio = 0;
                if (calificaciones.Count() > 0)
                {
                    calificacionPromedio = calificaciones.Average();
                }  
                ViewBag.calificacionPromedio = calificacionPromedio;
                if (usuario == null)
                {
                    return RedirectToAction("Index");
                }
                return View(usuario);

            }
        }
    }
}