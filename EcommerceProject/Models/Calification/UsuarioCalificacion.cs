﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    [Table("UsuariosXCalificaciones")]
    public class UsuarioCalificacion
    {
        [Key]
        public int Id { get; set; }
        public int Puntaje { get; set; }
        public String Comentario { get; set; }
        //public String Replica { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}