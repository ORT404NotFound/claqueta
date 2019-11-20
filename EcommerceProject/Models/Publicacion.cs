using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models
{
    [Table("Publicaciones")]
    public class Publicacion
    {
        public Publicacion()
        {
            this.Consultas = new HashSet<Consulta>();
            this.PublicacionCalificaciones = new HashSet<PublicacionCalificacion>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "Categor�a")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public String Categoria { get; set; }

        [Display(Name = "Disponibilidad")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public String Disponibilidad { get; set; }

        [Display(Name = "Ubicaci�n")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Text)]
        public String Ubicacion { get; set; }

        [Display(Name = "T�tulo")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Text)]
        public String Titulo { get; set; }

        [Display(Name = "Descripci�n")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.MultilineText)]
        public String Descripcion { get; set; }

        [Display(Name = "Foto")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.ImageUrl)]
        public String Foto { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Currency)]
        public double Precio { get; set; }

        [Display(Name = "Referencias")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Text)]
        public String Referencias { get; set; }

        [Display(Name = "Curriculum Vitae")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Url)]
        public String CV { get; set; }

        [Display(Name = "Reel")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Url)]
        public String Reel { get; set; }

        [Display(Name = "Fecha de Publicaci�n")]
        [DataType(DataType.Date)]
        public DateTime FechaDePublicacion { get; set; }

        [Display(Name = "Fecha de Modificaci�n")]
        [DataType(DataType.Date)]
        public DateTime FechaDeModificacion { get; set; }

        public bool Promocionada { get; set; }

        public bool Visible { get; set; }

        public String Estado { get; set; }

        public virtual Usuario Usuario { get; set; }

        [ForeignKey("Usuario")]
        public int Usuario_Id { get; set; }

        ///
        /// RELACIONES
        ///

        // RELACI�N DE 1 A MUCHOS POR CONSULTAS
        public ICollection<Consulta> Consultas { get; set; }

        // RELACI�N DE 1 A MUCHOS POR CALIFICACIONES
        public ICollection<PublicacionCalificacion> PublicacionCalificaciones { get; set; }
    }
}