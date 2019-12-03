using System;
using System.Linq;
using System.Web.Mvc;
using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using EcommerceProject.MPApi;

namespace EcommerceProject.Controllers
{
    public class MercadoPagoController : Controller
    {
        public ActionResult Pagar(Publicacion publicacion)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int usuarioId = Int32.Parse(Session["UserId"].ToString());

            using (var db = new SQLServerContext())
            {
                Usuario usuario = db.Usuarios.Find(usuarioId);

                MP mp = new MP();

                String url = mp.PagarPromocion(usuario, publicacion);

                return Redirect(url);
            }
        }

        public ActionResult PagarContratacion(int contratacionId)
        {
            int usuarioId = Int32.Parse(Session["UserId"].ToString());

            using (var db = new SQLServerContext())
            {
                Contratacion contratacion = db.Contrataciones
                    .Include("Publicacion")
                    .Include("FechaContratacion")
                    .FirstOrDefault(c => c.Id == contratacionId);

                Usuario usuario = db.Usuarios.Find(usuarioId);

                MP mp = new MP();

                String url = mp.PagarContratacion(usuario, contratacion);

                return Redirect(url);
            }
        }

        public ActionResult PagoError()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult PagoPendiente()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult PagoExitoso()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            String externalReference = Request.QueryString["external_reference"];
            int publicacionId = Int32.Parse(externalReference);
            int usuarioId = Int32.Parse(Session["UserId"].ToString());

            using (var db = new SQLServerContext())
            {
                Publicacion publicacion = db.Publicaciones.Find(publicacionId);
                publicacion.FechaDeModificacion = Convert.ToDateTime(DateTime.Now);
                publicacion.Promocionada = true;

                Usuario usuario = db.Usuarios.Find(usuarioId);

                Pago pago = new Pago
                {
                    Aprobado = true,
                    Concepto = "Promoción",
                    FechaDePago = Convert.ToDateTime(DateTime.Now),
                    Publicacion = publicacion,
                    Usuario = usuario
                };

                db.Pagos.Add(pago);
                db.SaveChanges();

                return View();
            }
        }

        public ActionResult PagoExitosoContratacion()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            String externalReference = Request.QueryString["external_reference"];
            int contratacionId = Int32.Parse(externalReference);
            int usuarioId = Int32.Parse(Session["UserId"].ToString());

            using (var db = new SQLServerContext())
            {
                Contratacion contratacion = db.Contrataciones.Include("Publicacion").FirstOrDefault(c => c.Id == contratacionId);
                contratacion.Estado = "Contratada";

                Usuario usuario = db.Usuarios.Find(usuarioId);

                Pago pago = new Pago
                {
                    Aprobado = true,
                    Concepto = "Contratación",
                    FechaDePago = Convert.ToDateTime(DateTime.Now),
                    Publicacion = contratacion.Publicacion,
                    Usuario = usuario
                };

                contratacion.Pago = pago;

                db.Pagos.Add(pago);
                db.SaveChanges();

                return View(contratacion.Publicacion.Usuario);
            }
        }
    }
}