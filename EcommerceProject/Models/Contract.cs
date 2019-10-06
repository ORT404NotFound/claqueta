using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceProject.Models
{
    public class Contract
    {
        [Key, ForeignKey("Payment")]
        public int Id { get; set; }
        public String State { get; set; }

        [ForeignKey("Publication")]
        public int PublicationId { get; set; }
        public Publication Publication { get; set; }

        public virtual Payment Payment { get; set; }
    }
}