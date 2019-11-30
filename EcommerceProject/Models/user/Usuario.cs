using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        public Usuario()
        {
            this.Roles = new HashSet<Rol>();
            this.Publicaciones = new HashSet<Publicacion>();
            this.Contrataciones = new HashSet<Contratacion>();
            this.UsuarioCalificacion = new HashSet<UsuarioCalificacion>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Text)]
        public String Nombre { get; set; }

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Text)]
        public String Apellido { get; set; }

        [Display(Name = "Tipo de Identificación")]
        public String TipoDocumento { get; set; }

        [Display(Name = "Número de Identificación")]
        public String Documento { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Date)]
        public DateTime? FechaDeNacimiento { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.PhoneNumber)]
        public String Telefono { get; set; }

        [Display(Name = "E-Mail")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Display(Name = "Confirmar Contraseña")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        [NotMapped]
        public String ConfirmPassword { get; set; }

        [Required]
        public bool Activo { get; set; }

        ///
        /// RELACIONES
        ///

        // RELACIÓN DE 1 A 1 POR ROL
        public virtual ICollection<Rol> Roles { get; set; }

        // RELACIÓN DE 1 A MUCHOS POR PUBLICACIONES
        public ICollection<Publicacion> Publicaciones { get; set; }

        // RELACIÓN DE 1 A MUCHOS POR CONTRATACIONES
        public ICollection<Contratacion> Contrataciones { get; set; }

        // RELACIÓN DE 1 A MUCHOS POR CALIFICACIONES
        public ICollection<UsuarioCalificacion> UsuarioCalificacion { get; set; }
    }
}