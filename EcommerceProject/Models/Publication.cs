namespace EcommerceProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Publication
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public int Featured { get; set; }    
        public virtual User Users { get; set; }
    }
}
