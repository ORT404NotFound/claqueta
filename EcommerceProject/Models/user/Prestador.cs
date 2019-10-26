using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    public class Prestador : Usuario
    {
        [Display(Name = "Poliza de Seguro")]
        public String PolizaDeSeguro { get; set; }
    }
}