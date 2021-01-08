using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.ActivityScheduler.Entities;


namespace WebAPI.ActivityScheduler.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "489184c8-447c-4d0b-9d82-1b4bd5a16b5a",
                    Name = UserRoles.StandardUser,
                    NormalizedName = UserRoles.NormalizedStandardUser
                },
                new IdentityRole
                {
                    Id = "eaed2ed5-57ac-44b1-8c1d-0aab8388b1b9",
                    Name = UserRoles.PremiumUser,
                    NormalizedName = UserRoles.NormalizedPremiumUser
                },
                new IdentityRole
                {
                    Id = "06ea2e08-bd95-4121-960a-650bd14fc326",
                    Name = UserRoles.Admin,
                    NormalizedName = UserRoles.NormalizedAdmin
                }
            );
        }
    }
}
