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
        public String Pagar(User u, Publication publication)
        {
            Environment.SetEnvironmentVariable("MP_ACCESS_TOKEN", "TEST-7861638524601067-100603-29811dd016706b7463468ecffe4a41ac-158446926");
            MercadoPago.SDK.CleanConfiguration();
            MercadoPago.SDK.AccessToken = Environment.GetEnvironmentVariable("MP_ACCESS_TOKEN");


            // Crea un objeto de preferencia
            Preference preference = new Preference();

            // Crea un ítem en la preferencia
            preference.Items.Add(
              new Item()
              {
                  Title = publication.Description,
                  Id = publication.Id.ToString(),
                  Quantity = 1,
                  CurrencyId = CurrencyId.ARS,
                  UnitPrice = (decimal)75.56
              }
            );
            preference.Payer = new Payer()
            {
                Email = u.Email,
            };
            
            // Save and posting preference
            preference.Save();

            return preference.InitPoint;

        }

     
    }
}