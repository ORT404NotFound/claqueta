using System.Data.Entity.Migrations;

namespace EcommerceProject.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Models.EcommerceProject.Models.SQLServerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Models.EcommerceProject.Models.SQLServerContext context)
        {
            MockData.Initialize();
            // This method will be called after migrating to the latest version.
            // You can use the DbSet<T>.AddOrUpdate() helper extension method to avoid creating duplicate seed data.
        }
    }
}