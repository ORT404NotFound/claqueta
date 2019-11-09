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

            }



        }

    }
}