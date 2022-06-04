using FinancialChat.IoC;
using FinancialChat.Web.Configuration;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAppConfiguration(builder.Configuration);
builder.Services.AddMassTransitConfiguration(builder.Configuration);
builder.Services.AddScoped<SignInManager<IdentityUser>>();

//Dependencies Injection
builder.Services.BuildConfiguration();


// Add App Middlewares
var app = builder.Build();



app.UseAppConfiguration();

app.UseMassTransitConfiguration();

//Dependencies Injection
app.BuildAppConfigure();


app.Run();
