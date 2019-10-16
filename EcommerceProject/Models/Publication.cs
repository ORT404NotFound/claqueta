namespace EcommerceProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Publication
    {


        ///
        /// RELACIONES 
        /// 
        public Publication()
        {
            this.Questions = new HashSet<Question>();
            this.PublicationCalifications = new HashSet<PublicationCalification>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Categoria es requerido")]
        [Display(Name = "Categoría")]
        public String Category { get; set; }

        [Display(Name = "Ubicación")]
        [Required(ErrorMessage = "El campo Ubicación es requerido")]
        [DataType(DataType.Text)]
        public String Location { get; set; }


        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo Descripción es requerido")]
        [DataType(DataType.MultilineText)]
        public String Description { get; set; }

        [Display(Name = "Foto")]
        [Required(ErrorMessage = "El campo Foto es requerido")]
        [DataType(DataType.ImageUrl)]
        public String Photo { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo Precio es requerido")]
        [DataType(DataType.Text)]
        public double Price { get; set; }

        [Display(Name = "Referencias")]
        [Required(ErrorMessage = "El campo Referencias es requerido")]
        [DataType(DataType.Text)]
        public String References { get; set; }

        [Display(Name = "CV")]
        [Required(ErrorMessage = "El campo CV es requerido")]
        [DataType(DataType.Url)]
        public String CV { get; set; }

        [Display(Name = "Reel")]
        [Required(ErrorMessage = "El campo Reel es requerido")]
        [DataType(DataType.Text)]
        public String Reel { get; set; }

        [Display(Name = "Garantía")]
        [Required(ErrorMessage = "El campo Garantía es requerido")]
        [DataType(DataType.Currency)]
        public double Warranty { get; set; }


        [Display(Name = "Promocionada")]
        public bool Featured { get; set; }


        [Display(Name = "Visible")]
        public bool Visible { get; set; }


        [Display(Name = "Estado")]
        public String State { get; set; }





        ///
        /// RELACIONES 
        /// 
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        // relacion 1 a muchos por consultas 
        public ICollection<Question> Questions { get; set; }

        public ICollection<PublicationCalification> PublicationCalifications { get; set; }


    }
}
