using EcommerceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            using (var ctx = new SQLServerContext()) {

                /// guardo los datos basicos 
                var user = new User() {
                    FirstName = "Mario",
                    LastName = "Sarasa"
                };
                
                ctx.Users.Add(user);

                UserType userTypeOne = new UserType();
                userTypeOne.Type = "User";
                UserType userTypeTwo = new UserType();
                userTypeTwo.Type = "Seller";

                ctx.UserTypes.Add(userTypeOne);
                ctx.UserTypes.Add(userTypeTwo);

                ctx.SaveChanges();

                //creo la relacion y modifico 

                UserType firstUserType = ctx.UserTypes.Where(b => b.Type == "User").First();
                user.UserTypes.Add(firstUserType);

                User MyUser = ctx.Users.Where(u => u.FirstName == "Mario").First();

                if (MyUser != null) {
                    MyUser.LastName = "Pirulo";
                }
                ctx.SaveChanges();
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}