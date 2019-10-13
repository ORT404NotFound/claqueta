using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    public class UserCalification
    {
        [Key]
        public int Id { get; set; }
        public int Score { get; set; }
        public String Comment { get; set; }
        public String Replica { get; set; }
        public virtual User User { get; set; }
    }
}