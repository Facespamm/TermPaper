using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resend;
using TermPaper.Application.Common;
using TermPaper.Application.Interface;
using TermPaper.Context;
using TermPaper.Models;
using TermPaper.Application.Interface;
using TermPaper.Infrastructure.Services;
using TermPaper.Infrastructure.Settings;

namespace TermPaper.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddIdentity<AppUser, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = true)
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        var googleSettings = configuration.GetSection("Authentication:Google")
            .Get<GoogleAuthenticationSettings>()?? new GoogleAuthenticationSettings();
        services.AddAuthentication().AddGoogle(options =>
        {
            options.ClientId = googleSettings.ClientId;
            options.ClientSecret = googleSettings.ClientSecret;    
        });
        services.AddScoped<IRegisteredServices, RegisteredServices>();
        services.AddScoped<ILoginService,LoginService>();
        services.AddResend(options => options.ApiToken = Environment.GetEnvironmentVariable("RESEND_TOKEN"));

        return services;
    }
 
}