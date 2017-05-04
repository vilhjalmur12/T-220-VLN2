using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using CodeEditorApp.Migrations;
using CodeEditorApp.Models.Entities;

namespace CodeEditorApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        // Setja lista hér:


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
           // Database.SetInitializer<ApplicationDbContext>(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           base.OnModelCreating(modelBuilder);
            // modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }

      //  public System.Data.Entity.DbSet<CodeEditorApp.Models.AspNetUser> AspNetUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectType> ProjectTypes { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Entities.RootFolder> RootFolders { get; set; }
    }
}   
    

