namespace EcommerceProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Service
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public int Featured { get; set; }    
        public virtual User Users { get; set; }
    }
}
