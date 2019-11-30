using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models
{
    [Table("PublicacionesXCalificaciones")]
    public class PublicacionCalificacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Puntaje { get; set; }

        public String Comentario { get; set; }

        [Required]
        public virtual Publicacion Publicacion { get; set; }
    }
}