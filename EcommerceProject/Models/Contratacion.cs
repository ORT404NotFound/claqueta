using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    [Table("Contrataciones")]
    public class Contratacion
    { 
        [Key]
        public int Id { get; set; }
        public String Estado { get; set; }

        [ForeignKey("Publicacion")]
        public int PublicacionId { get; set; }
        public Publicacion Publicacion { get; set; }

        public virtual Pago Pago { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

    }
}