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
            this.Califications = new HashSet<Calification>();
            this.Questions = new HashSet<Question>();

        }


        [Key]
        public int Id { get; set; }

        public String Category { get; set; }
        public String Location { get; set; }
        public String Description { get; set; }
        public String Photo { get; set; }
        public double Price { get; set; }
        public String References { get; set; }
        public String CV { get; set; }
        public String Reel { get; set; }
        public double Warranty { get; set; }
        public bool Featured { get; set; }
        public bool Visible { get; set; }




        ///
        /// RELACIONES 
        /// 

        public virtual User User { get; set; }

        public ICollection<Calification> Califications { get; set; }


        // relacion 1 a muchos por consultas 
        public ICollection<Question> Questions { get; set; }

    }
}
