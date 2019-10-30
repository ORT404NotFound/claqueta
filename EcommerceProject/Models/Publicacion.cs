namespace EcommerceProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("Publicaciones")]
    public class Publicacion
    {


        ///
        /// RELACIONES 
        /// 
        public Publicacion()
        {
            this.Consultas = new HashSet<Consulta>();
            this.PublicacionCalicicationes = new HashSet<PublicacionCalificacion>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Categoria es requerido")]
        [Display(Name = "Categor�a")]
        public String Categoria { get; set; }

        [Display(Name = "Ubicaci�n")]
        [Required(ErrorMessage = "El campo Ubicaci�n es requerido")]
        [DataType(DataType.Text)]
        public String Ubicacion { get; set; }


        [Display(Name = "Descripci�n")]
        [Required(ErrorMessage = "El campo Descripci�n es requerido")]
        [DataType(DataType.MultilineText)]
        public String Descripcion { get; set; }

        [Display(Name = "Foto")]
        [Required(ErrorMessage = "El campo Foto es requerido")]
        [DataType(DataType.ImageUrl)]
        public String Foto { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo Precio es requerido")]
        [DataType(DataType.Text)]
        public double Precio { get; set; }

        [Display(Name = "Referencias")]
        [Required(ErrorMessage = "El campo Referencias es requerido")]
        [DataType(DataType.Text)]
        public String Referencias { get; set; }

        [Display(Name = "CV")]
        [Required(ErrorMessage = "El campo CV es requerido")]
        [DataType(DataType.Url)]
        public String CV { get; set; }

        [Display(Name = "Reel")]
        [Required(ErrorMessage = "El campo Reel es requerido")]
        [DataType(DataType.Text)]
        public String Reel { get; set; }

        [Display(Name = "Garant�a")]
        [Required(ErrorMessage = "El campo Garant�a es requerido")]
        [DataType(DataType.Currency)]
        public double Garantia { get; set; }


        [Display(Name = "Promocionada")]
        public bool Promocicionada { get; set; }


        [Display(Name = "Visible")]
        public bool Visible { get; set; }


        [Display(Name = "Estado")]
        public String Estado { get; set; }





        ///
        /// RELACIONES 
        /// 
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; }

        // relacion 1 a muchos por consultas 
        public ICollection<Consulta> Consultas { get; set; }

        public ICollection<PublicacionCalificacion> PublicacionCalicicationes { get; set; }
    }
}
