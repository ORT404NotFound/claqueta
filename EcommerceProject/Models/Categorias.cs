using System;

namespace EcommerceProject.Models
{
    public static class Categorias
    {
        public static String[] GetCategorias()
        {
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

            return categorias;
        }
    }
}