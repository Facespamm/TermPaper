using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TermPaper.Components;
using TermPaper.Context;
using TermPaper.Models;
using TermPaper.Services;
using Resend;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDbContext<AppDbContext>(options =>options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<AppUser,IdentityRole>(options=>options.SignIn.RequireConfirmedEmail=true).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<RegisterServices>();
builder.Services.AddScoped<AutorisationsServices>();
builder.Services.AddResend(options=>options.ApiToken=Environment.GetEnvironmentVariable("RESEND_TOKEN"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();