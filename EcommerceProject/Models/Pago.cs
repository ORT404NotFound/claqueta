using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    public class Pago
    {
        [Key, ForeignKey("Contratacion")]
        public int Id { get; set; }
        public bool Aprobado { get; set; }
        public int ContratacionId { get; set; }
        public virtual Contratacion Contratacion { get; set; }
    }
}