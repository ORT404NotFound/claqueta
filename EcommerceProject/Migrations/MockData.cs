﻿using EcommerceProject.Models;
using EcommerceProject.Models.EcommerceProject.Models;
using System;
using System.Linq;
using System.Web.Helpers;

namespace EcommerceProject.Migrations
{
    public class MockData
    {
        public static void Initialize()
        {
            String[] roles = new String[] { "USER", "ADMIN" };

            String[] categorias = new String[] {
                "Asistentes de Producción",
                "Camarógrafos",
                "Castineras",
                "Drones",
                "Editores",
                "Efectos FX",
                "Equipos de Audio",
                "Equipos de Video",
                "Escenógrafos",
                "Extras",
                "Fotógrafos",
                "Iluminadores",
                "Locaciones Privadas",
                "Maquilladores",
                "Postproducción",
                "Productores",
                "Sonidistas",
                "Utileros",
                "Vestuaristas"
            };

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

                String passwordAdmin = Crypto.HashPassword("contra#123");

                if (usuario == null)
                {
                    Usuario user = new Usuario
                    {
                        Nombre = "Administrador",
                        Apellido = "Claqueta",
                        Password = passwordAdmin,
                        ConfirmPassword = passwordAdmin,
                        Activo = true
                    };

                    Rol rol = db.Roles.SingleOrDefault(r => r.Nombre == "ADMIN");

                    user.Roles.Add(rol);
                    user.Email = "admin@claqueta.com.ar";
                    user.FechaDeNacimiento = Convert.ToDateTime("01/01/2000");
                    user.Telefono = "12345678";
                    user.TipoDocumento = "DNI";
                    user.Documento = "12345678";

                    db.Usuarios.Add(user);
                    db.SaveChanges();
                }

                foreach (var categoria in categorias)
                {
                    Categoria cat = db.Categorias.SingleOrDefault(c => c.Nombre == categoria);

                    if (cat == null)
                    {
                        // SI LA CATEGORÍA NO EXISTE LA GUARDA EN LA BASE DE DATOS
                        Categoria miCategoria = new Categoria()
                        {
                            Nombre = categoria
                        };
                        db.Categorias.Add(miCategoria);
                    }
                }
                db.SaveChanges();
            }
        }
    }
}