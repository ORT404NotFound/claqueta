using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    [Table("Consultas")]
    public class Consulta
    {
        [Key]
        public int Id { get; set; }
        public bool Visible { get; set; }
        public String Descripcion { get; set; }
        public String Respuesta { get; set; }
        public virtual Usuario Usuario { get; set; }

    }
}