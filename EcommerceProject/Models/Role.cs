using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    public class Role
    {
        public Role()
        {
            this.Users = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string Rolename { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}