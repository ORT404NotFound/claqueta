using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    [Table("Pagos")]
    public class Pago
    {
        [Key]
        public int Id { get; set; }
        public bool Aprobado { get; set; }
        public String Concepto { get; set; }
        public virtual Publicacion Publicacion { get; set; }
        public virtual Usuario Usuario { get; set; }
        public DateTime FechaDePago { get; set; }
    }
}