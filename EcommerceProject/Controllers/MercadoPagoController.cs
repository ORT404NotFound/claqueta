using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using EcommerceProject.MPApi;
using MercadoPago.Common;
using MercadoPago.DataStructures.Preference;
using MercadoPago.Resources;
namespace EcommerceProject.Controllers
{
    public class MercadoPagoController : Controller
    {
        public ActionResult Pagar (Publicacion p)
        {
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
                Usuario u = db.Usuarios.Find(userId);
                MP mp = new MP();
                String url = mp.Pagar(u, p);
                return Redirect(url);
            }
        }
        public ActionResult PagoError ()
        {
            return View();
        }

        public ActionResult PagoPendiente ()
        {
            return View();
        }

        public ActionResult PagoExitoso ()
        {
            String externalRef = Request.QueryString["external_reference"];
            int userId;
            try
            {
                userId = Int32.Parse(Session["UserId"].ToString());
            }
            catch (FormatException e)
            {
                throw e;
            }


            int publicationId;
            try
            {
                publicationId = Int32.Parse(externalRef);
            }
            catch (FormatException e)
            {
                throw e;
            }
            using (var db = new SQLServerContext())
            {
                Usuario u = db.Usuarios.Find(userId);
                Publicacion p = db.Publicaciones.Find(publicationId);
                p.FechaDeModificacion = Convert.ToDateTime(DateTime.Now);
                p.Promocionada = true;
                
                //creo un pago para una promocion
                Pago pago = new Pago();
                pago.Aprobado = true;
                pago.Concepto = "PROMOCION";
                pago.Usuario = u;
                pago.Publicacion = p;
                pago.FechaDePago = Convert.ToDateTime(DateTime.Now);
                
                db.Pagos.Add(pago);

                //guardo cambios en la db
                db.SaveChanges();
                return RedirectToAction("UserInfo","Account");
            }
        }
    }
}