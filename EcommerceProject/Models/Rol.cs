﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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
        public string Nombre { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}