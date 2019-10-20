using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcommerceProject.Models;
using EcommerceProject.MPApi;
using MercadoPago.Common;
using MercadoPago.DataStructures.Preference;
using MercadoPago.Resources;
namespace EcommerceProject.Controllers
{
    public class MercadoPagoController : Controller
    {
        public ActionResult Pagar (User u,Publication p)
        {
            MP mp = new MP();
            String url = mp.Pagar(u, p);

            return Redirect(url);
            //return RedirectToAction("GoToMercadoPago", url);
        }
    }
}