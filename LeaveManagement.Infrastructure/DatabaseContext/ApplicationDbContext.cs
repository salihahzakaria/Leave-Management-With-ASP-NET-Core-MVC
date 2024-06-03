using LeaveManagement.Core.Domain.Entities;
using LeaveManagement.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Infrastructure.DatabaseContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<LeaveType> LeaveType { get; set; }
        public virtual DbSet<Leave> Leave { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LeaveType>().ToTable("LeaveType");
            modelBuilder.Entity<Leave>().ToTable("Leave");

            //Seed data to leavetype
            string leavetypesJson = File.ReadAllText("leavetypes.json");
            List<LeaveType> leaveTypes = System.Text.Json.JsonSerializer.Deserialize<List<LeaveType>>(leavetypesJson);

            foreach (LeaveType leaveType in leaveTypes)
            {
                modelBuilder.Entity<LeaveType>().HasData(leaveType);
            }
        }
    }
}
