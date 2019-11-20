using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models
{
    [Table("Roles")]
    public class Rol
    {
        public Rol()
        {
            this.Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }

        public String Nombre { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}