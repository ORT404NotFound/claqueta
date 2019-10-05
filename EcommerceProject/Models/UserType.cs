using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    public class UserType
    {
        public UserType()
        {
            this.Users = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }
        public String Type { get; set; }


        // relacion muchos a muchos con User
        public virtual ICollection<User> Users { get; set; }

    }
}