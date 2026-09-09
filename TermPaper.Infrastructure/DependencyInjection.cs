using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resend;
using TermPaper.Context;
using TermPaper.Models;
using TermPaper.Services;

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

        services.AddScoped<AutorisationsServices>();

        services.AddResend(options => options.ApiToken = Environment.GetEnvironmentVariable("RESEND_TOKEN"));

        return services;
    }
 
}