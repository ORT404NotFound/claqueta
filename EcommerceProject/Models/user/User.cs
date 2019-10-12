using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    public class User
    {

        ///
        /// RELACIONES 
        /// 
        public User()
        {
            this.Publications = new HashSet<Publication>();
            this.Califications = new HashSet<Calification>();
            this.Contracts = new HashSet<Contract>();
        }

        [Key]
        public int Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Active { get; set; }


        ///
        /// RELACIONES 
        /// 
        // relacion 1 a muchos por Publications (a confirmar talvez es muchos a muchos)
        public ICollection<Publication> Publications { get; set; }

        // relacion 1 a muchos por calificaciones 
        public ICollection<Calification> Califications { get; set; }

        // relacion 1 a muchos por contrataciones 

        public ICollection<Contract> Contracts { get; set; }


    }
}