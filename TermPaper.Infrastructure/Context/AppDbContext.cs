using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TermPaper.Models;

namespace TermPaper.Context;

public class AppDbContext: IdentityDbContext<AppUser, IdentityRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<AppUser>()
            .Property(u => u.Locale)
            .HasConversion<string>();
        builder.Entity<AppUser>()
            .Property(u=>u.Theme)
            .HasConversion<string>();
    }
}



