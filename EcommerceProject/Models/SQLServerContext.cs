using System.Data.Entity;

namespace EcommerceProject.Models
{
    namespace EcommerceProject.Models
    {
        public class SQLServerContext : DbContext
        {
            // public SQLServerContext() : base("name=awsConn")
            public SQLServerContext() : base("Claqueta")
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<SQLServerContext, Migrations.Configuration>());
            }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
            }

            public DbSet<Categoria> Categorias { get; set; }

            public DbSet<Consulta> Consultas { get; set; }

            public DbSet<Contratacion> Contrataciones { get; set; }

            public DbSet<FechaContratacion> FechasXContratacion { get; set; }

            public DbSet<Pago> Pagos { get; set; }

            public DbSet<Publicacion> Publicaciones { get; set; }

            public DbSet<PublicacionCalificacion> PublicacionesXCalificaciones { get; set; }

            public DbSet<Rol> Roles { get; set; }

            public DbSet<Usuario> Usuarios { get; set; }

            public DbSet<UsuarioCalificacion> UsuariosXCalificaciones { get; set; }
        }
    }
}