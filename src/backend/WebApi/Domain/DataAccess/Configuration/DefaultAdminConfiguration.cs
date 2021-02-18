using ActivityScheduler.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ActivityScheduler.Domain.DataAccess
{
    public class DefaultAdminConfiguration : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            var hasher = new PasswordHasher<User>();
            builder.HasData(
                new User
                {
                    Id = "b90f5900-6d5a-4f71-b4b5-aa2424a60a5d",
                    UserName = "admin",
                    LastName = "adminsky",
                    IsAdmin = true,
                    NormalizedUserName = "ADMIN",
                    PasswordHash = hasher.HashPassword(null, "Admin#3"),
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM"
                }
            );
        }
    }
}
