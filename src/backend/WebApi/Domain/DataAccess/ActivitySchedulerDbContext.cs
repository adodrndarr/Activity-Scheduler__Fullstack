using ActivityScheduler.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace ActivityScheduler.Domain.DataAccess
{
    public class ActivitySchedulerDbContext: IdentityDbContext<User>
    {
        public ActivitySchedulerDbContext(DbContextOptions options): base(options)
        {
        }


        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityEntity> ActivityEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new DefaultAdminConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        }
    }
}
