using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceProject.Controllers
{
    public class UserController : Controller
    {
        

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                using (var ctx = new SQLServerContext())
                {
                    var user = ctx.Users.Find(id);
                    return View(user);
                }
            }
            catch (Exception e) {
                throw e;
            }
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                using (var ctx = new SQLServerContext())
                {
                    var user = ctx.Users.Find(id);
                    return View(user);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                using (var ctx = new SQLServerContext())
                {
                    var user = ctx.Users.Find(id);
                    return View(user);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            { 
                using (var ctx = new SQLServerContext())
                {
                    var user = ctx.Users.Find(id);
                    ctx.Users.Remove(user);
                    ctx.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
