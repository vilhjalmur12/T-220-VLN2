namespace CodeEditorApp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CodeEditorApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            // Ef notaður er remote database eins og RU database þá = true
            // Ef notaður er local database þá = false
            AutomaticMigrationsEnabled = false;

           // AutomaticMigrationDataLossAllowed = true;

        }

        protected override void Seed(CodeEditorApp.Models.ApplicationDbContext context)
        {
            

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
