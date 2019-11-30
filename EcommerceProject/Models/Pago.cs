using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models
{
    [Table("Pagos")]
    public class Pago
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public bool Aprobado { get; set; }

        [Required]
        public String Concepto { get; set; }

        [Required]
        public DateTime FechaDePago { get; set; }

        [Required]
        public virtual Publicacion Publicacion { get; set; }

        // [Required]
        public virtual Usuario Usuario { get; set; }
    }
}