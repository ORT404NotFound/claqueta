using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public bool Visible { get; set; }
        public String Description { get; set; }
        public String Response { get; set; }
        public virtual User User { get; set; }

    }
}