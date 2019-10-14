using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
            this.UserCalifications = new HashSet<UserCalification>();
            this.Contracts = new HashSet<Contract>();
            this.Roles = new HashSet<Role>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name="Email")]
        [Required(ErrorMessage = "El campo email es requerido")]
        public String Email { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo contraseña es requerido")]
        [DataType(DataType.Password)] 
        [StringLength(255)]
        public String Password { get; set; }

        [Compare("Password", ErrorMessage = "Las contraseñas deben coincidir")]
        [Display(Name = "Confirmar Contraseña")]
        [DataType(DataType.Password)]
        [NotMapped]
        [StringLength(255)]
        public String ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El campo nombre es requerido")]
        [Display(Name = "Nombre")]
        public String FirstName { get; set; }


        [Required(ErrorMessage = "El campo apellido es requerido")]
        [Display(Name = "Apellido")]
        public String LastName { get; set; }


        [Display(Name = "Fecha de nacimiento")]
        [Required(ErrorMessage = "El campo fecha de nacimiento es requerido")]
        [DataType(DataType.DateTime)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Activo")]
        public int Active { get; set; }


        ///
        /// RELACIONES 
        /// 
        // relacion 1 a muchos por Publications (a confirmar talvez es muchos a muchos)
        public ICollection<Publication> Publications { get; set; }

        // relacion 1 a muchos por calificaciones 
        public ICollection<UserCalification> UserCalifications { get; set; }

        // relacion 1 a muchos por contrataciones 

        public ICollection<Contract> Contracts { get; set; }

        public virtual ICollection<Role> Roles { get; set; }



    }
}