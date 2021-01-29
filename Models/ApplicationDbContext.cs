using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskDone_V2.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Status> Status { get; set; }
        public ApplicationDbContext()
             : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}