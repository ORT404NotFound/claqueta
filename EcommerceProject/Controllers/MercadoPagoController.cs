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
        public ActionResult Pagar (Publication p)
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
                User u = db.Users.Find(userId);
                MP mp = new MP();
                String url = mp.Pagar(u, p);
                return Redirect(url);
            }
            //return RedirectToAction("GoToMercadoPago", url);
        }
    }
}