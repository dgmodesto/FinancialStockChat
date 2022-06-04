using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinancialChatBackend.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // any guid
            const string ADMIN_ID = "3370475C-F265-4CBF-8FDE-07C04FBFF15E";
            // any guid, but nothing is against to use the same one
            const string ROLE_ID = ADMIN_ID;
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = ROLE_ID,
                Name = "admin",
                NormalizedName = "admin"
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });

            var hasher = new PasswordHasher<IdentityUser>();
            builder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = ADMIN_ID,
                UserName = "bot@financialchat.com",
                NormalizedUserName = "Bot",
                Email = "bot@financialchat.com",
                NormalizedEmail = "bot@financialchat.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "ChatBotPassword@123"),
                SecurityStamp = string.Empty
            });

      
        }
    }
}