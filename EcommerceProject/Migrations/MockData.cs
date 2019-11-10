using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceProject.Migrations
{
    public class MockData
    {
        public static void Initialize()
        {
            string[] roles = new string[] { "USER", "ADMIN" };
            using (var db = new SQLServerContext())
            {
                foreach (var role in roles) {
                    Rol ro = db.Roles.SingleOrDefault(r => r.Nombre == role);
                    if (ro == null) {
                        // si el rol no existe lo guarda en la db
                        Rol myRole = new Rol()
                        {
                            Nombre = role
                        };
                        db.Roles.Add(myRole);
                    }
                }
                db.SaveChanges();
                Usuario us = db.Usuarios.Where(u => u.Email == "admin@claqueta.com.ar").FirstOrDefault();
                if (us == null) {
                    Usuario user = new Usuario();
                    user.Activo = 1;
                    user.Apellido = "Claqueta";
                    user.Nombre = "Administrador";
                    user.Password = "123123";
                    user.ConfirmPassword = "123123";
                    Rol rol = db.Roles.SingleOrDefault(r => r.Nombre == "ADMIN");
                    user.Roles.Add(rol);
                    user.Telefono = "48612121";
                    user.TipoDocumento = "DNI";
                    user.Documento = "111222333";
                    user.Email = "admin@claqueta.com.ar";
                    user.FechaDeNacimiento = Convert.ToDateTime(DateTime.Now);
                    db.Usuarios.Add(user);
                    db.SaveChanges();

                }
            }

         

        }

    }
}