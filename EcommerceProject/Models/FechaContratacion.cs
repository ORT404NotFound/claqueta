using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models
{
    [Table("FechasXContratacion")]
    public class FechaContratacion
    {
        public Contratacion Contratacion { get; set; }

        [Key, Column(Order = 0)]
        [ForeignKey("Contratacion")]
        public int Contratacion_Id { get; set; }

        [Key, Column(Order = 1)]
        public DateTime Fecha { get; set; }
    }
}