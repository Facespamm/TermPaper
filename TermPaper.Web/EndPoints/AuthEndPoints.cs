using Microsoft.AspNetCore.Identity;
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
    }
}