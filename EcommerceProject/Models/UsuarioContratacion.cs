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
        [Column(Order = 1), Key]
        public int UsuarioId { get; set; }
        [Column(Order = 2), Key]
        public int ContratacionId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Contratacion Contratacion { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
    }
}