using System.Data.Entity;

namespace EcommerceProject.Models
{
    namespace EcommerceProject.Models
    {
        public class SQLServerContext : DbContext
        {
            //public SQLServerContext() : base("name=awsConn")
            public SQLServerContext() : base("ecommerce")
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<SQLServerContext, Migrations.Configuration>());
            }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

            }
            public DbSet<User> Users { get; set; }
            public DbSet<Role> Roles { get; set; }
            public DbSet<UserCalification> Califications { get; set; }
            public DbSet<Contract> Contracts { get; set; }
            public DbSet<Payment> Payments { get; set; }
            public DbSet<Publication> Publications { get; set; }
            public DbSet<Question> Questions { get; set; }
        }
    }
}