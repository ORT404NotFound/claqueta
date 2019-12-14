using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceProject.Controllers
{
    public class CalificationController : Controller
    {
        [HttpPost]
        public ActionResult CalificarPrestadorAPrestatario(FormCollection formulario)
        {
            
            if (Session["UserId"] == null)
            {
                return Json("NotAuthorized");
            }

            int calificacion = Int32.Parse(formulario["calificacion"]);
            int usuarioIdACalificar = Int32.Parse(formulario["usuarioACalificar"]);
            int contratacionId = Int32.Parse(formulario["contratacionId"]);

            String comentario = formulario["comentario"];


            if (calificacion > 5 || calificacion < 1)
            {
                return Json("Error");
            }

            using (var db = new SQLServerContext())
            {
                var usuarioACalificar = db.Usuarios.Find(usuarioIdACalificar);
                var contratatacion = db.Contrataciones.Find(contratacionId);
                var usuarioCalificacion = new UsuarioCalificacion();
                usuarioCalificacion.Puntaje = calificacion;
                usuarioCalificacion.Comentario = comentario;
                usuarioCalificacion.Usuario = usuarioACalificar;

                db.UsuariosXCalificaciones.Add(usuarioCalificacion);
                db.SaveChanges();
                return Json("OK");
            }
        }

        [HttpPost]
        public ActionResult CalificarPrestatarioAPrestadorYAPublicacion(FormCollection formulario)
        {
            if (Session["UserId"] == null)
            {
                return Json("NotAuthorized");
            }

            int calificacionPrestador = Int32.Parse(formulario["calificacionPrestador"]);
            int calificacionPublicacion = Int32.Parse(formulario["calificacionPublicacion"]);
            int usuarioIdACalificar = Int32.Parse(formulario["usuarioACalificar"]);
            int contratacionId = Int32.Parse(formulario["contratacionId"]);
            int publicacionId = Int32.Parse(formulario["publicacionId"]);

            String comentario = formulario["comentario"];


            if (calificacionPublicacion > 5 || calificacionPublicacion < 1)
            {
                return Json("Error");
            }

            if (calificacionPrestador > 5 || calificacionPrestador < 1)
            {
                return Json("Error");
            }

            using (var db = new SQLServerContext())
            {
                var usuarioACalificar = db.Usuarios.Find(usuarioIdACalificar);
                var contratatacion = db.Contrataciones.Find(contratacionId);
                var publicacion = db.Publicaciones.Find(publicacionId);

                var usuarioCalificacion = new UsuarioCalificacion();
                usuarioCalificacion.Puntaje = calificacionPrestador;
                usuarioCalificacion.Comentario = comentario;
                usuarioCalificacion.Usuario = usuarioACalificar;

                var publicacionCalificacion = new PublicacionCalificacion();
                publicacionCalificacion.Puntaje = calificacionPublicacion;
                publicacionCalificacion.Comentario = comentario;
                publicacionCalificacion.Publicacion = publicacion;


                db.UsuariosXCalificaciones.Add(usuarioCalificacion);
                db.PublicacionesXCalificaciones.Add(publicacionCalificacion);

                db.SaveChanges();
                return Json("OK");
            }
        }
    }
}