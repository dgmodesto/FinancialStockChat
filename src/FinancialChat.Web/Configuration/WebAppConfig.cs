using FinancialChatBackend.Data;
using FinancialChatBackend.Hubs;
using FinancialChatBackend.Integration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinancialChat.Web.Configuration
{
    public static class WebAppConfig
    {
        public static void AddAppConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<ApplicationDbContext>();


            services.AddScoped<IStooqIntegrationService, StooqIntegrationService>();


            services.AddRazorPages();
            services.AddSignalR();


        }


        public static void UseAppConfiguration(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                //Same as the question
                db.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.MapHub<StockChatHub>("/chat");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

        }

    }
}
