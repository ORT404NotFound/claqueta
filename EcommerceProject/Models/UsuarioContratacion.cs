using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    public class UsuarioContratacion
    {
        [Key]
        public int Id { get; set; }
        public virtual Contratacion Contratacion { get; set; }
        public virtual Usuario Usuario { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
    }
}