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

        [Display(Name = "Categoría")]
        public virtual Categoria Categoria { get; set; }

        [ForeignKey("Categoria")]
        public int Categoria_Id { get; set; }

        [Display(Name = "Disponibilidad")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public String Disponibilidad { get; set; }

        [Display(Name = "Ubicación")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Text)]
        public String Ubicacion { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Text)]
        public String Titulo { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.MultilineText)]
        public String Descripcion { get; set; }

        [Display(Name = "Foto")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Upload)]
        public String Foto { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Currency)]
        public double Precio { get; set; }

        [Display(Name = "Referencias")]
        [DataType(DataType.Text)]
        public String Referencias { get; set; }

        [Display(Name = "Curriculum Vitae")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Upload)]
        public String CV { get; set; }

        [Display(Name = "Reel")]
        [DataType(DataType.Url)]
        public String Reel { get; set; }

        [Display(Name = "Fecha de Publicación")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaDePublicacion { get; set; }

        [Display(Name = "Fecha de Modificación")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaDeModificacion { get; set; }

        [Required]
        public bool Promocionada { get; set; }

        [Required]
        public bool Visible { get; set; }

        [Required]
        public String Estado { get; set; }

        public virtual Usuario Usuario { get; set; }

        [ForeignKey("Usuario")]
        public int Usuario_Id { get; set; }

        ///
        /// RELACIONES
        ///

        // RELACIÓN DE 1 A MUCHOS POR CONSULTAS
        public ICollection<Consulta> Consultas { get; set; }

        // RELACIÓN DE 1 A MUCHOS POR CALIFICACIONES
        public ICollection<PublicacionCalificacion> PublicacionCalificaciones { get; set; }
    }
}