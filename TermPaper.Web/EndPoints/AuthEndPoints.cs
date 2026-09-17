using Microsoft.AspNetCore.Identity;
using TermPaper.Application.Common;
using TermPaper.Interface;
using TermPaper.Models;

namespace TermPaper.EndPoints;

public static class AuthEndPoints
{
    public static void MapAuthEndpoints( this WebApplication app)
    {
        app.MapGet("/api/auth/google-login", (SignInManager<AppUser> signInManager) =>
            {
                var redirectUrl = "/api/auth/google-callback";
                var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
                return Results.Challenge(properties, new[] { "Google" });
            }
        );
        app.MapGet("/api/auth/google-callback", async (IExternalAuthService externalAuthService) =>
        {
            var result = await externalAuthService.ExternalLoginAsync();
            return result.IsSuccess ? Results.Redirect("/") : Results.Redirect($"/LoginPage?error={result.ErrorCode}");
        });

        app.MapGet("/api/auth/facebook-login", (SignInManager<AppUser> signInManager) =>
        {
            var redirectUrl = "/api/auth/facebook-callback";
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return Results.Challenge(properties, new[] { "Facebook" });
        });

        app.MapGet("/api/auth/facebook-callback", async (IExternalAuthService externalAuthService) =>
        {
            var result = await externalAuthService.ExternalLoginAsync();
            return result.IsSuccess ? Results.Redirect("/") : Results.Redirect($"/LoginPage?error={result.ErrorCode}");
        });

        app.MapGet("/api/auth/log-out", async (SignInManager<AppUser> signInManager) =>
        {
            await signInManager.SignOutAsync();
            return Results.Redirect("/LoginPage");
        });
    }
}