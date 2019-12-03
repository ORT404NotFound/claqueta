using System;
using System.Linq;
using EcommerceProject.Models;
using MercadoPago.Common;
using MercadoPago.DataStructures.Preference;
using MercadoPago.Resources;

namespace EcommerceProject.MPApi
{
    public class MP
    {
        // MODIFICAR ESTO DEPENDE DEL AMBIENTE A UTILIZAR
        public String ambiente = "Desarrollo";

        public String PagarPromocion(Usuario usuario, Publicacion publicacion)
        {
            Environment.SetEnvironmentVariable("MP_ACCESS_TOKEN", "TEST-7861638524601067-100603-29811dd016706b7463468ecffe4a41ac-158446926");
            MercadoPago.SDK.CleanConfiguration();
            MercadoPago.SDK.AccessToken = Environment.GetEnvironmentVariable("MP_ACCESS_TOKEN");

            String siteURL;

            if (ambiente == "Desarrollo")
            {
                siteURL = "http://localhost:55115";
            }
            else
            {
                siteURL = "http://ec2-3-82-109-216.compute-1.amazonaws.com/";
            }

            double valorDolar = 65;
            double precioPromocion = 4 * valorDolar;

            // CREA UN OBJETO DE PREFERENCIA
            Preference preference = new Preference();

            // CREA UN ÍTEM EN LA PREFERENCIA
            preference.Items.Add(new Item()
            {
                Title = publicacion.Titulo,
                Id = publicacion.Id.ToString(),
                Quantity = 1,
                CurrencyId = CurrencyId.ARS,
                UnitPrice = (decimal)precioPromocion
            });

            preference.Payer = new Payer()
            {
                Email = usuario.Email,
            };

            preference.ExternalReference = publicacion.Id.ToString();

            preference.BackUrls = new BackUrls()
            {
                Success = siteURL + "/MercadoPago/PagoExitoso",
                Failure = siteURL + "/MercadoPago/PagoError",
                Pending = siteURL + "/MercadoPago/PagoPendiente"
            };

            preference.AutoReturn = AutoReturnType.approved;

            // GUARDA Y PUBLICA EL OBJETO DE PREFERENCIA
            preference.Save();

            return preference.InitPoint;
        }

        public String PagarContratacion(Usuario usuario, Contratacion contratacion)
        {
            Environment.SetEnvironmentVariable("MP_ACCESS_TOKEN", "TEST-7861638524601067-100603-29811dd016706b7463468ecffe4a41ac-158446926");
            MercadoPago.SDK.CleanConfiguration();
            MercadoPago.SDK.AccessToken = Environment.GetEnvironmentVariable("MP_ACCESS_TOKEN");

            String siteURL;

            if (ambiente == "Desarrollo")
            {
                siteURL = "http://localhost:55115";
            }
            else
            {
                siteURL = "http://ec2-3-82-109-216.compute-1.amazonaws.com/";
            }

            double precioPublicacion = contratacion.Publicacion.Precio;
            int cantidadDeDias = contratacion.FechaContratacion.Count();
            double precioContratacion = precioPublicacion * cantidadDeDias;

            // CREA UN OBJETO DE PREFERENCIA
            Preference preference = new Preference();

            // CREA UN ÍTEM EN LA PREFERENCIA
            preference.Items.Add(new Item()
            {
                Title = contratacion.Publicacion.Titulo,
                Id = contratacion.Id.ToString(),
                Quantity = 1,
                CurrencyId = CurrencyId.ARS,
                UnitPrice = (decimal)precioContratacion
            });

            preference.Payer = new Payer()
            {
                Email = usuario.Email,
            };

            preference.ExternalReference = contratacion.Id.ToString();

            preference.BackUrls = new BackUrls()
            {
                Success = siteURL + "/MercadoPago/PagoExitosoContratacion",
                Failure = siteURL + "/MercadoPago/PagoError",
                Pending = siteURL + "/MercadoPago/PagoPendiente"
            };

            preference.AutoReturn = AutoReturnType.approved;

            // GUARDA Y PUBLICA EL OBJETO DE PREFERENCIA
            preference.Save();

            return preference.InitPoint;
        }
    }
}