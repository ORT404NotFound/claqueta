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
        [RegularExpression(@"^[A-ZñÑáéíóúÁÉÍÓÚ]+[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]{2,50}$", ErrorMessage = "Nombre no válido.")]
        public String Nombre { get; set; }

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[A-ZñÑáéíóúÁÉÍÓÚ]+[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]{1,50}$", ErrorMessage = "Apellido no válido.")]
        public String Apellido { get; set; }

        [Display(Name = "Tipo de Identificación")]
        public String TipoDocumento { get; set; }

        [Display(Name = "Número de Identificación")]
        [RegularExpression(@"^[0-9]{7,11}$", ErrorMessage = "Ingrese un número de 7 a 11 caracteres.")]
        public String Documento { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaDeNacimiento { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]{6,14}$", ErrorMessage = "Ingrese un número de 6 a 14 caracteres.")]
        public String Telefono { get; set; }

        [Display(Name = "E-Mail")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Ingrese un E-Mail válido.")]
        public String Email { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Debe contener un número, una letra y un caracter especial. Longitud: 8 a 15 caracteres.")]
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