using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models
{
    [Table("Consultas")]
    public class Consulta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public bool Visible { get; set; }

        [Required]
        public String Descripcion { get; set; }

        public String Respuesta { get; set; }

        [Required]
        public virtual Publicacion Publicacion { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}