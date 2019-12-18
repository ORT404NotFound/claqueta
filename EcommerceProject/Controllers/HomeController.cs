﻿using EcommerceProject.Models.EcommerceProject.Models;
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
                var publicacionesPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == true)
                    .OrderByDescending(p => p.FechaDeModificacion).ToList();
                var publicacionesNoPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == false)
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
                    .Include("PublicacionCalificaciones")
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
                    var calificaciones = db.UsuariosXCalificaciones.Where(uc => uc.Usuario.Id == publicacion.Usuario.Id).Select(x => x.Puntaje);

                    Double calificacionPromedio = 0;

                    if (calificaciones.Count() > 0)
                    {
                        calificacionPromedio = calificaciones.Average();
                    }

                    ViewBag.CalificacionPromedio = Math.Round(calificacionPromedio, 2);

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

                var contrataciones = db.Contrataciones.Include("FechaContratacion").Where(c => c.FechaContratacion.Any(fc => fc.Reservada == true && fc.Contratacion.Publicacion.Id == publicacion.Id)).ToArray();

                if (contrataciones != null)
                {
                    List<DateTime> fechas = new List<DateTime>();

                    foreach (var contratacion in contrataciones)
                    {
                        if (contratacion.FechaContratacion.Count > 0)
                        {
                            foreach (var fechaContratacion in contratacion.FechaContratacion)
                            {
                                fechas.Add(fechaContratacion.Fecha);
                            }
                        }
                    }

                    return Json(fechas.ToArray(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("NOTOK", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult BuscarPublicacionesPorCategoria(int categoriaId, Double precioMinimo = 0, Double precioMaximo = 0)
        {
            using (var db = new SQLServerContext())
            {
                // ÚNICAMENTE FILTRA POR PRECIO MÍNIMO
                if (precioMinimo > 0 && precioMaximo == 0)
                {
                    var categoria = db.Categorias.Where(c => c.Id == categoriaId).FirstOrDefault();

                    var publicacionesPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == true && p.Categoria.Id == categoriaId && p.Precio >= precioMinimo)
                        .OrderByDescending(p => p.FechaDeModificacion).ToList();
                    var publicacionesNoPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == false && p.Categoria.Id == categoriaId && p.Precio >= precioMinimo)
                        .OrderByDescending(p => p.FechaDeModificacion).ToList();
                    var publicaciones = publicacionesPromocionadas.Concat(publicacionesNoPromocionadas).ToList();

                    ViewBag.CategoriaPublicacion = categoria.Nombre;

                    return View("BuscadorPublicaciones", publicaciones);
                }
                // ÚNICAMENTE FILTRA POR PRECIO MÁXIMO
                else if (precioMaximo > 0 && precioMinimo == 0)
                {
                    var categoria = db.Categorias.Where(c => c.Id == categoriaId).FirstOrDefault();

                    var publicacionesPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == true && p.Categoria.Id == categoriaId && precioMaximo <= p.Precio)
                        .OrderByDescending(p => p.FechaDeModificacion).ToList();
                    var publicacionesNoPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == false && p.Categoria.Id == categoriaId && precioMaximo <= p.Precio)
                        .OrderByDescending(p => p.FechaDeModificacion).ToList();
                    var publicaciones = publicacionesPromocionadas.Concat(publicacionesNoPromocionadas).ToList();

                    ViewBag.CategoriaPublicacion = categoria.Nombre;

                    return View("BuscadorPublicaciones", publicaciones);
                }
                // FILTRA POR PRECIO MÍNIMO Y PRECIO MÁXIMO
                else if (precioMaximo > 0 && precioMinimo > 0)
                {
                    var categoria = db.Categorias.Where(c => c.Id == categoriaId).FirstOrDefault();

                    var publicacionesPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == true && p.Categoria.Id == categoriaId && p.Precio >= precioMinimo && precioMaximo <= p.Precio)
                        .OrderByDescending(p => p.FechaDeModificacion).ToList();
                    var publicacionesNoPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == false && p.Categoria.Id == categoriaId && p.Precio >= precioMinimo && precioMaximo <= p.Precio)
                        .OrderByDescending(p => p.FechaDeModificacion).ToList();
                    var publicaciones = publicacionesPromocionadas.Concat(publicacionesNoPromocionadas).ToList();

                    ViewBag.CategoriaPublicacion = categoria.Nombre;

                    return View("BuscadorPublicaciones", publicaciones);
                }
                // SIN FILTRO DE PRECIO
                else
                {
                    var categoria = db.Categorias.Where(c => c.Id == categoriaId).FirstOrDefault();

                    var publicacionesPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == true && p.Categoria.Id == categoriaId)
                        .OrderByDescending(p => p.FechaDeModificacion).ToList();
                    var publicacionesNoPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == false && p.Categoria.Id == categoriaId)
                        .OrderByDescending(p => p.FechaDeModificacion).ToList();
                    var publicaciones = publicacionesPromocionadas.Concat(publicacionesNoPromocionadas).ToList();

                    ViewBag.CategoriaPublicacion = categoria.Nombre;

                    return View("BuscadorPublicaciones", publicaciones);
                }
            }
        }

        public ActionResult BuscarPublicaciones(String termino, Double precioMinimo = 0, Double precioMaximo = 0)
        {
            using (var db = new SQLServerContext())
            {
                // ÚNICAMENTE FILTRA POR PRECIO MÍNIMO
                if (precioMinimo > 0 && precioMaximo == 0)
                {
                    var publicacionesPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == true && p.Titulo.ToLower().Contains(termino.ToLower()) && p.Precio >= precioMinimo)
                        .OrderByDescending(p => p.FechaDeModificacion).ToList();
                    var publicacionesNoPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == false && p.Titulo.ToLower().Contains(termino.ToLower()) && p.Precio >= precioMinimo)
                        .OrderByDescending(p => p.FechaDeModificacion).ToList();
                    var publicaciones = publicacionesPromocionadas.Concat(publicacionesNoPromocionadas).ToList();

                    ViewBag.Termino = termino;

                    return View("BuscadorPublicaciones", publicaciones);
                }
                // ÚNICAMENTE FILTRA POR PRECIO MÁXIMO
                else if (precioMaximo > 0 && precioMinimo == 0)
                {
                    var publicacionesPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == true && p.Titulo.ToLower().Contains(termino.ToLower()) && precioMaximo <= p.Precio)
                        .OrderByDescending(p => p.FechaDeModificacion).ToList();
                    var publicacionesNoPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == false && p.Titulo.ToLower().Contains(termino.ToLower()) && precioMaximo <= p.Precio)
                        .OrderByDescending(p => p.FechaDeModificacion).ToList();
                    var publicaciones = publicacionesPromocionadas.Concat(publicacionesNoPromocionadas).ToList();

                    ViewBag.Termino = termino;

                    return View("BuscadorPublicaciones", publicaciones);
                }
                // FILTRA POR PRECIO MÍNIMO Y PRECIO MÁXIMO
                else if (precioMaximo > 0 && precioMinimo > 0)
                {
                    var publicacionesPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == true && p.Titulo.ToLower().Contains(termino.ToLower()) && p.Precio >= precioMinimo && precioMaximo <= p.Precio)
                        .OrderByDescending(p => p.FechaDeModificacion).ToList();
                    var publicacionesNoPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == false && p.Titulo.ToLower().Contains(termino.ToLower()) && p.Precio >= precioMinimo && precioMaximo <= p.Precio)
                        .OrderByDescending(p => p.FechaDeModificacion).ToList();
                    var publicaciones = publicacionesPromocionadas.Concat(publicacionesNoPromocionadas).ToList();

                    ViewBag.Termino = termino;

                    return View("BuscadorPublicaciones", publicaciones);
                }
                // SIN FILTRO DE PRECIO
                else
                {
                    var publicacionesPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == true && p.Titulo.ToLower().Contains(termino.ToLower()))
                        .OrderByDescending(p => p.FechaDeModificacion).ToList();
                    var publicacionesNoPromocionadas = db.Publicaciones.Where(p => p.Visible == true && p.Estado == "Aprobada" && p.Promocionada == false && p.Titulo.ToLower().Contains(termino.ToLower()))
                        .OrderByDescending(p => p.FechaDeModificacion).ToList();
                    var publicaciones = publicacionesPromocionadas.Concat(publicacionesNoPromocionadas).ToList();

                    ViewBag.Termino = termino;

                    return View("BuscadorPublicaciones", publicaciones);
                }
            }
        }

        public ActionResult PerfilUsuario(int usuarioId)
        {
            if (usuarioId == 1)
            {
                return RedirectToAction("Index");
            }

            using (var db = new SQLServerContext())
            {
                var usuario = db.Usuarios.Include("UsuarioCalificacion").SingleOrDefault(u => u.Id == usuarioId);
                var calificaciones = db.UsuariosXCalificaciones.Where(uc => uc.Usuario.Id == usuario.Id).Select(x => x.Puntaje);

                if (usuario == null)
                {
                    return RedirectToAction("Index");
                }

                Double calificacionPromedio = 0;

                if (calificaciones.Count() > 0)
                {
                    calificacionPromedio = calificaciones.Average();
                }

                ViewBag.CalificacionPromedio = Math.Round(calificacionPromedio, 2);

                return View(usuario);
            }
        }
    }
}