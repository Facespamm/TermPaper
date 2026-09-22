using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resend;
using TermPaper.Application.Interface;
using TermPaper.Infrastructure.Context;
using TermPaper.Models;
using TermPaper.Infrastructure.Services;
using TermPaper.Infrastructure.Settings;
using TermPaper.Interface;
using Microsoft.Extensions.Options;
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
        var facebookSettings = configuration.GetSection("Authentication:Facebook").Get<FaceBookAuthentificationSettings>() ?? new FaceBookAuthentificationSettings();
        services.AddAuthentication().AddFacebook(options =>
        {
            options.AppId = facebookSettings.AppId;
            options.AppSecret = facebookSettings.AppSecret;
        });
        services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
        services.AddScoped<IRegisterService, RegisterService>();
        services.AddScoped<IPositionService, PositionService>();
        services.AddScoped<IAttributeService , AttributeService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<CloudinaryService>();
        services.AddScoped<IUserManagementService, UserManagementService>();
        services.AddScoped<ISenderEmail, SenderEmailSerivce>();
        services.AddScoped<ILoginService,LoginService>();
        services.AddResend(options => options.ApiToken = Environment.GetEnvironmentVariable("RESEND_TOKEN"));
        services.AddScoped<IExternalAuthService, ExternalAuthService>();

        return services;
    }
 
}