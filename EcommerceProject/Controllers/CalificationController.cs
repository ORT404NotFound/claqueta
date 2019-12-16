using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using System;
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
            String comentario = formulario["comentario"];
            int usuarioIdACalificar = Int32.Parse(formulario["usuarioACalificar"]);
            int contratacionId = Int32.Parse(formulario["contratacionId"]);

            if (calificacion > 5 || calificacion < 1)
            {
                return Json("Error");
            }

            using (var db = new SQLServerContext())
            {
                var usuario = db.Usuarios.Find(usuarioIdACalificar);
                var contratatacion = db.Contrataciones.Find(contratacionId);

                var usuarioCalificacion = new UsuarioCalificacion
                {
                    Puntaje = calificacion,
                    Comentario = comentario,
                    Usuario = usuario,
                    Contratacion = contratatacion
                };

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
            String comentarioPrestador = formulario["comentarioPrestador"];
            int calificacionPublicacion = Int32.Parse(formulario["calificacionPublicacion"]);
            String comentarioPublicacion = formulario["comentarioPublicacion"];
            int usuarioIdACalificar = Int32.Parse(formulario["usuarioACalificar"]);
            int publicacionId = Int32.Parse(formulario["publicacionId"]);
            int contratacionId = Int32.Parse(formulario["contratacionId"]);

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
                var usuario = db.Usuarios.Find(usuarioIdACalificar);
                var publicacion = db.Publicaciones.Find(publicacionId);
                var contratatacion = db.Contrataciones.Find(contratacionId);

                var usuarioCalificacion = new UsuarioCalificacion
                {
                    Puntaje = calificacionPrestador,
                    Comentario = comentarioPrestador,
                    Usuario = usuario,
                    Contratacion = contratatacion
                };

                var publicacionCalificacion = new PublicacionCalificacion
                {
                    Puntaje = calificacionPublicacion,
                    Comentario = comentarioPublicacion,
                    Publicacion = publicacion,
                    Contratacion = contratatacion
                };

                db.UsuariosXCalificaciones.Add(usuarioCalificacion);
                db.PublicacionesXCalificaciones.Add(publicacionCalificacion);

                db.SaveChanges();

                return Json("OK");
            }
        }
    }
}