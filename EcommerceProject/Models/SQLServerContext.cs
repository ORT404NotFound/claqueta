using System.Data.Entity;

namespace EcommerceProject.Models
{
    public class SQLServerContext : DbContext
    {
        public SQLServerContext() : base("name=awsConn")
        //public SQLServerContext() : base("ecommerce")
        {

            //crea una db si no existe
            Database.SetInitializer<SQLServerContext>(new CreateDatabaseIfNotExists<SQLServerContext>());
            //si se modifica el modelo se borra y se recrea la db (solo para dev) 
            Database.SetInitializer<SQLServerContext>(new DropCreateDatabaseIfModelChanges<SQLServerContext>());
        }
        public DbSet<User> Users { get; set; }
    }
}