using FinancialChat.IoC;
using FinancialChat.Web.Configuration;
using FinancialChatBackend.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAppConfiguration(builder.Configuration);
builder.Services.AddMassTransitConfiguration(builder.Configuration);
builder.Services.AddSignalRConfiguration(builder.Configuration);

//Dependencies Injection
builder.Services.BuildConfiguration();


// Add App Middlewares
var app = builder.Build();

app.UseAppConfiguration();
app.UseMassTransitConfiguration();
app.UseSignalRConfiguration();

//Dependencies Injection
app.BuildAppConfigure();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
    context.Database.Migrate();
}
app.Run();
