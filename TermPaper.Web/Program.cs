    using System.Text.Json.Serialization;
    using Microsoft.AspNetCore.Identity;
    using Radzen;
    using TermPaper.Components;
    using TermPaper.Infrastructure;
    using TermPaper.EndPoints;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.EntityFrameworkCore;
    using TermPaper.Infrastructure.Identity;
    using TermPaper.Infrastructure.Settings;

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddScoped<DialogService>();
    builder.Services.AddRadzenComponents();
    builder.Services.AddAuthorization();
    builder.Services.AddCascadingAuthenticationState();
    builder.Services.ConfigureHttpJsonOptions(o =>
        o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    var app = builder.Build();

    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        KnownIPNetworks = { },
        KnownProxies = { }
    });

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<TermPaper.Infrastructure.Context.AppDbContext>();
        await dbContext.Database.MigrateAsync();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roles = { "Candidate", "Recruiter", "Admin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
        
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var hasAdmin = (await userManager.GetUsersInRoleAsync("Admin")).Any();
        if (!hasAdmin)
        {
            var adminEmail = app.Configuration["Admin:Email"] ?? "admin@termpaper.local";
            var adminPassword = app.Configuration["Admin:Password"] ?? "Admin123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var createResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                    app.Logger.LogError("Failed to seed default admin user: {Errors}", errors);
                }
            }

            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }

    app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

    if (app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

    app.UseAntiforgery();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapStaticAssets();
    app.MapAuthEndpoints();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Run();