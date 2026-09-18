using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TermPaper.Domain.Models;
using TermPaper.Models;

namespace TermPaper.Infrastructure.Context;

public class AppDbContext: IdentityDbContext<AppUser, IdentityRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.Property(u => u.Locale).HasConversion<string>();
            entity.Property(u => u.Theme).HasConversion<string>();
        });

        modelBuilder.Entity<Attributes>(entity =>
            {
                entity.Property(u => u.DataType).HasConversion<string>();
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasOne(a => a.Category)
                    .WithMany(b => b.AttributesList)
                    .HasForeignKey(a => a.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        );

        modelBuilder.Entity<AttributeEnumValue>(entity =>
        {
            entity.HasOne<Attributes>()
                .WithMany(a => a.AttributeEnumValues)
                .HasForeignKey(a => a.AtributeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserAtributes>(entity =>
        {
            entity.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<Attributes>()
                .WithMany(a => a.UserAtributes)
                .HasForeignKey(a => a.AtributeId)
                .OnDelete(DeleteBehavior.Cascade);
            
        });
        

    }
    
    public virtual DbSet<Attributes>  Attributes { get; set; }
    
    public virtual DbSet<AttributeEnumValue> AttributeEnumValues { get; set; }
    
    public virtual DbSet<UserAtributes>  UserAttributes { get; set; }
    
    public virtual DbSet<AttributeCategory> AttributeCategories { get; set; }
    
    
}



