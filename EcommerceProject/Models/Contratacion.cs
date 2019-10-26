using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    public class Contratacion
    {
        [Key, ForeignKey("Pago")]
        public int Id { get; set; }
        public String Estado { get; set; }

        [ForeignKey("Publicacion")]
        public int PublicacionId { get; set; }
        public Publicacion Publicacion { get; set; }

        public virtual Pago Pago { get; set; }
    }
}