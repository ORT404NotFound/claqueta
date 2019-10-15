namespace EcommerceProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

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
        [DataType(DataType.Text)]
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


        [Display(Name = "Destacado")]
        [DataType(DataType.Currency)]
        public bool Featured { get; set; }


        [Display(Name = "Visible")]
        [DataType(DataType.Currency)]
        public bool Visible { get; set; }


        [Display(Name = "Estado")]
        [DataType(DataType.Currency)]
        public String State { get; set; }





        ///
        /// RELACIONES 
        /// 

        public virtual User User { get; set; }

        // relacion 1 a muchos por consultas 
        public ICollection<Question> Questions { get; set; }

        public ICollection<PublicationCalification> PublicationCalifications { get; set; }


    }
}
