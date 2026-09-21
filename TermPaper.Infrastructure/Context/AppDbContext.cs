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

        modelBuilder.Entity<AttributeValueOption>(entity =>
        {
            entity.HasOne<Attributes>()
                .WithMany(a => a.AttributeValueOptions)
                .HasForeignKey(a => a.AttributeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserAttributes>(entity =>
        {
            entity.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<Attributes>()
                .WithMany(a => a.UserAttributes)
                .HasForeignKey(a => a.AttributeId)
                .OnDelete(DeleteBehavior.Cascade);
            
        });

        modelBuilder.Entity<RecentlyUsedAttribute>(entity =>
        {
            entity.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(a => a.UserId);
            entity.HasOne<Attributes>()
                .WithMany()
                .HasForeignKey(a => a.AttributeId);
        });
    }
    
    public virtual DbSet<Attributes>  Attributes { get; set; }
    
    public virtual DbSet<AttributeValueOption> AttributeValueOptions { get; set; }
    
    public virtual DbSet<UserAttributes>  UserAttributes { get; set; }
    
    public virtual DbSet<RecentlyUsedAttribute> RecentlyUsedAttribute { get; set; }
    
    public virtual DbSet<AttributeCategory> AttributeCategories { get; set; }
    
    public virtual DbSet<Position> Positions { get; set; }
    
    public virtual DbSet<PositionAttribute> PositionAttributes { get; set; }
    
    public virtual DbSet<PositionProjectTag> PositionProjectTags { get; set; }
    
    public virtual DbSet<AccessRule> AccessRules { get; set; }
    
    public virtual DbSet<Projects>  Projects { get; set; }
    
    public virtual DbSet<ProjectTags>  ProjectTags { get; set; }
    
    
    
    
    
    
    
    
}



