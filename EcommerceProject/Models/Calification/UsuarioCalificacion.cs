using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models
{
    [Table("UsuariosXCalificaciones")]
    public class UsuarioCalificacion
    {
        [Key]
        public int Id { get; set; }

        public int Puntaje { get; set; }

        public String Comentario { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}