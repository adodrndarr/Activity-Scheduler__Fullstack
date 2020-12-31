using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace WebAPI.ActivityScheduler.Configuration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            const string adminUserId = "b90f5900-6d5a-4f71-b4b5-aa2424a60a5d";
            const string standardRoleId = "489184c8-447c-4d0b-9d82-1b4bd5a16b5a";
            const string premiumRoleId = "eaed2ed5-57ac-44b1-8c1d-0aab8388b1b9";
            const string adminRoleId = "06ea2e08-bd95-4121-960a-650bd14fc326";

            builder.HasData(
                new IdentityUserRole<string>
                {
                    UserId = adminUserId,
                    RoleId = standardRoleId
                },
                new IdentityUserRole<string>
                {
                    UserId = adminUserId,
                    RoleId = premiumRoleId
                },
                new IdentityUserRole<string>
                {
                    UserId = adminUserId,
                    RoleId = adminRoleId
                }
            );
        }
    }
}
