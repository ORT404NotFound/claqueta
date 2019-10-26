using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    public class Usuario
    {

        ///
        /// RELACIONES 
        /// 
        public Usuario()
        {
            this.Publicationes = new HashSet<Publicacion>();
            this.UsuarioCalificacion = new HashSet<UsuarioCalificacion>();
            this.Contrataciones = new HashSet<Contratacion>();
            this.Roles = new HashSet<Rol>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name="Email")]
        [Required(ErrorMessage = "El campo email es requerido")]
        public String Email { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo contraseña es requerido")]
        [DataType(DataType.Password)] 
        [StringLength(255)]
        public String Password { get; set; }

        [Compare("Password", ErrorMessage = "Las contraseñas deben coincidir")]
        [Display(Name = "Confirmar Contraseña")]
        [DataType(DataType.Password)]
        [NotMapped]
        [StringLength(255)]
        public String ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El campo nombre es requerido")]
        [Display(Name = "Nombre")]
        public String Nombre { get; set; }


        [Required(ErrorMessage = "El campo apellido es requerido")]
        [Display(Name = "Apellido")]
        public String Apellido { get; set; }


        [Display(Name = "Fecha de nacimiento")]
        [Required(ErrorMessage = "El campo fecha de nacimiento es requerido")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaDeNacimiento { get; set; }

        [Display(Name = "Activo")]
        public int Activo { get; set; }

        [Display(Name = "Tipo de Documento")]
        [Required(ErrorMessage = "El campo Tipo de Documento es requerido")]
        public String TipoDocumento { get; set; }

        [Display(Name = "Documento")]
        [Required(ErrorMessage = "El campo Documento es requerido")]
        [DataType(DataType.Text)]
        public String Documento { get; set; }


        ///
        /// RELACIONES 
        /// 
        // relacion 1 a muchos por Publications (a confirmar talvez es muchos a muchos)
        public ICollection<Publicacion> Publicationes { get; set; }

        // relacion 1 a muchos por calificaciones 
        public ICollection<UsuarioCalificacion> UsuarioCalificacion { get; set; }

        // relacion 1 a muchos por contrataciones 

        public ICollection<Contratacion> Contrataciones { get; set; }

        public virtual ICollection<Rol> Roles { get; set; }



    }
}