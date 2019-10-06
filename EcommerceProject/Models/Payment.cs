using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public bool Aprobado { get; set; }

        public int ContractId { get; set; }
        public virtual Contract Contract { get; set; }

    }
}