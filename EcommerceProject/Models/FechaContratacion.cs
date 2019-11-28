using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    [Table("FechasXContratacion")]
    public class FechaContratacion
    {
        [Key]
        public int Id { get; set; }
        public Contratacion Contratacion { get; set; }

        [ForeignKey("Contratacion")]
        public int Contratacion_id { get; set; }
        public DateTime Fecha { get; set; }
    }
}