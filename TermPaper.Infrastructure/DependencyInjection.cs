using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Resend;
using TermPaper.Application.Interface;
using TermPaper.Infrastructure.Context;
using TermPaper.Infrastructure.Services;
using TermPaper.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using TermPaper.Infrastructure.Identity;

namespace TermPaper.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 1;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        var googleSettings = configuration.GetSection("Authentication:Google")
            .Get<GoogleAuthenticationSettings>()?? new GoogleAuthenticationSettings();
        services.AddAuthentication().AddGoogle(options =>
        {
            options.ClientId = googleSettings.ClientId;
            options.ClientSecret = googleSettings.ClientSecret;
            options.Events.OnRemoteFailure = RedirectToLoginOnRemoteFailure;
        });
        var facebookSettings = configuration.GetSection("Authentication:Facebook").Get<FacebookAuthenticationSettings>() ?? new FacebookAuthenticationSettings();
        services.AddAuthentication().AddFacebook(options =>
        {
            options.AppId = facebookSettings.AppId;
            options.AppSecret = facebookSettings.AppSecret;
            options.Events.OnRemoteFailure = RedirectToLoginOnRemoteFailure;
        });
        services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
        services.Configure<SalesforceSettings>(
            configuration.GetSection("Salesforce"));
        services.Configure<EntraSettings>(configuration.GetSection("Entra"));
        services.AddScoped<IRegisterService, RegisterService>();
        services.AddScoped<IPositionService, PositionService>();
        services.AddScoped<IAttributeService , AttributeService>();
        services.AddScoped<IProjectService, ProjectService>();  
        services.AddHttpClient<ICrmService, CrmService>();
        services.AddScoped<CloudinaryService>();
        services.AddScoped<IUserManagementService, UserManagementService>();
        services.AddScoped<ISenderEmailService, SenderEmailService>();
        services.AddScoped<ILoginService,LoginService>();
        services.AddResend(options =>
            options.ApiToken = configuration["Resend:ApiKey"]);
        
        services.AddScoped<IExternalAuthService, ExternalAuthService>();

        return services;
    }
    private static Task RedirectToLoginOnRemoteFailure(RemoteFailureContext context)
    {
        var logger = context.HttpContext.RequestServices
            .GetRequiredService<ILoggerFactory>().CreateLogger("ExternalAuth");
        logger.LogWarning(context.Failure, "External login failed");
        context.HandleResponse();
        context.Response.Redirect("/LoginPage");
        return Task.CompletedTask;
    } 
}