using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using System;
using System.Linq;

namespace EcommerceProject.Migrations
{
    public class MockData
    {
        public static void Initialize()
        {
            string[] roles = new string[] { "USER", "ADMIN" };

            using (var db = new SQLServerContext())
            {
                foreach (var rol in roles)
                {
                    Rol ro = db.Roles.SingleOrDefault(r => r.Nombre == rol);

                    if (ro == null)
                    {
                        // SI EL ROL NO EXISTE LO GUARDA EN LA BASE DE DATOS
                        Rol myRole = new Rol()
                        {
                            Nombre = rol
                        };
                        db.Roles.Add(myRole);
                    }
                }
                db.SaveChanges();

                Usuario usuario = db.Usuarios.Where(u => u.Email == "admin@claqueta.com.ar").FirstOrDefault();

                if (usuario == null)
                {
                    Usuario user = new Usuario
                    {
                        Nombre = "Administrador",
                        Apellido = "Claqueta",
                        Password = "123123",
                        ConfirmPassword = "123123",
                        Activo = true
                    };

                    Rol rol = db.Roles.SingleOrDefault(r => r.Nombre == "ADMIN");

                    user.Roles.Add(rol);
                    user.Email = "admin@claqueta.com.ar";
                    user.FechaDeNacimiento = Convert.ToDateTime("01/01/2000");
                    user.Telefono = "-";

                    db.Usuarios.Add(user);
                    db.SaveChanges();
                }
            }
        }
    }
}