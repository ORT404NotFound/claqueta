using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceProject.Migrations
{
    public class MockRoles
    {
        public static void Initialize()
        {
            string[] roles = new string[] { "USER", "ADMIN" };
            using (var db = new SQLServerContext())
            {
                foreach (var role in roles) {
                    Role ro = db.Roles.SingleOrDefault(r => r.Rolename == role);
                    if (ro == null) {
                        // si el rol no existe lo guarda en la db
                        Role myRole = new Role()
                        {
                            Rolename = role
                        };
                        db.Roles.Add(myRole);
                        db.SaveChanges();
                    }
                }
            } 
        }

    }
}