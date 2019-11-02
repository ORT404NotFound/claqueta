using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcommerceProject.Models;
using MercadoPago.Common;
using MercadoPago.DataStructures.Preference;
using MercadoPago.Resources;
namespace EcommerceProject.MPApi
{
    public class MP
    {
        public String Pagar(Usuario u, Publicacion publication)
        {
            Environment.SetEnvironmentVariable("MP_ACCESS_TOKEN", "TEST-7861638524601067-100603-29811dd016706b7463468ecffe4a41ac-158446926");
            MercadoPago.SDK.CleanConfiguration();
            MercadoPago.SDK.AccessToken = Environment.GetEnvironmentVariable("MP_ACCESS_TOKEN");

            //cambiar este depende el ambiente
            String siteURL = "http://localhost:55115";
            //String siteURL = "ec2-100-26-100-77.compute-1.amazonaws.com";

            double valorDolar = 80;
            double valorTotal = 4 * valorDolar;


            // Crea un objeto de preferencia
            Preference preference = new Preference();

            // Crea un ítem en la preferencia
            preference.Items.Add(
              new Item()
              {
                  Title = publication.Descripcion,
                  Id = publication.Id.ToString(),
                  Quantity = 1,
                  CurrencyId = CurrencyId.ARS,
                  UnitPrice = (decimal)valorTotal
              }
            ) ;
            preference.Payer = new Payer()
            {
                Email = u.Email,
            };
            preference.ExternalReference = publication.Id.ToString();

            preference.BackUrls = new BackUrls()
            {
                Success = siteURL+ "/MercadoPago/PagoExitoso",
                Failure = siteURL + "/MercadoPago/PagoError",
                Pending = siteURL + "MercadoPago/PagoPendiente"
            };
            preference.AutoReturn = AutoReturnType.approved;

            // Save and posting preference
            preference.Save();
            return preference.InitPoint;
        }

     
    }
}