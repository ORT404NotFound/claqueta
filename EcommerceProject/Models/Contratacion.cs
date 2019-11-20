using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models
{
    [Table("Contrataciones")]
    public class Contratacion
    {
        [Key]
        public int Id { get; set; }

        public String Estado { get; set; }

        public String Fechas { get; set; }

        public Publicacion Publicacion { get; set; }

        [ForeignKey("Publicacion")]
        public int Publicacion_Id { get; set; }

        public virtual Pago Pago { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}