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
