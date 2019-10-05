using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    public class User
    {

        public User()
        {
            this.Services = new HashSet<Publication>();
        }

        [Key]
        public int Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Active { get; set; }


        // relacion 1 a muchos por posts (a confirmar talvez es muchos a muchos)
        public ICollection<Publication> Publications { get; set; }
    }
}