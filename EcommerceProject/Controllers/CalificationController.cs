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
                contratatacion.Calificada = true;
                db.UsuariosXCalificaciones.Add(usuarioCalificacion);
                db.SaveChanges();
                return Json("OK");
            }
        }

        [HttpPost]
        public ActionResult CalificarPrestatarioAPrestador(FormCollection formulario)
        {
            if (Session["UserId"] == null)
            {
                return Json("NotAuthorized");
            }
            using (var db = new SQLServerContext())
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult CalificarPrestatarioAPublicacion(FormCollection formulario)
        {
            if (Session["UserId"] == null)
            {
                return Json("NotAuthorized");
            }
            using (var db = new SQLServerContext())
            {
                return null;
            }
        }
    }
}