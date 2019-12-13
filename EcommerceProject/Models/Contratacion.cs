using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models
{
    [Table("Contrataciones")]
    public class Contratacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public String Estado { get; set; }

        public bool Calificada { get; set; }

        public Publicacion Publicacion { get; set; }

        [ForeignKey("Publicacion")]
        public int Publicacion_Id { get; set; }

        public virtual Pago Pago { get; set; }

        // [Required]
        public virtual Usuario Usuario { get; set; }

        public ICollection<FechaContratacion> FechaContratacion { get; set; }
    }
}