using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TermPaper.Domain.Models;
using TermPaper.Enum;
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
        });

        modelBuilder.Entity<AttributeValueOption>(entity =>
        {
            entity.HasOne(e => e.Attribute)
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

            entity.HasOne(e => e.Attributes)
                .WithMany(a => a.UserAttributes)
                .HasForeignKey(a => a.AttributeId)
                .OnDelete(DeleteBehavior.Cascade); 

            entity.HasIndex(e => new { e.UserId, e.AttributeId }).IsUnique();
        });

        modelBuilder.Entity<RecentlyUsedAttribute>(entity =>
        {
            entity.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Attribute)
                .WithMany()
                .HasForeignKey(a => a.AttributeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.UserId, e.AttributeId }).IsUnique();
        });

        modelBuilder.Entity<PositionAttribute>(entity =>
        {
            entity.HasOne(e => e.Position)
                .WithMany()
                .HasForeignKey(a => a.PositionId)
                .OnDelete(DeleteBehavior.Cascade); 

            entity.HasOne(e => e.Attributes)
                .WithMany()
                .HasForeignKey(a => a.AttributeId)
                .OnDelete(DeleteBehavior.Cascade); 

            entity.HasIndex(e => new { e.PositionId, e.AttributeId }).IsUnique();
        });

        modelBuilder.Entity<PositionProjectTag>(entity =>
        {
            entity.HasOne(e => e.Position)
                .WithMany()
                .HasForeignKey(a => a.PositionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AccessRule>(entity =>
        {
            entity.HasOne(e => e.Attribute)
                .WithMany()
                .HasForeignKey(a => a.AttributeId)
                .OnDelete(DeleteBehavior.Cascade); 

            entity.HasOne(e => e.Position)
                .WithMany()
                .HasForeignKey(a => a.PositionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Projects>(entity =>
        {
            entity.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade); 
        });

        modelBuilder.Entity<ProjectTags>(entity =>
        {
            entity.HasOne(e => e.Project)
                .WithMany()
                .HasForeignKey(a => a.ProjectId)
                .OnDelete(DeleteBehavior.Cascade); 
        });
        
        
        modelBuilder.Entity<AttributeCategory>().HasData(
            new AttributeCategory { Id = 1, Name = "Personal" }
        );

        modelBuilder.Entity<Attributes>().HasData(
            new Attributes { Id = 1, CategoryId = 1, Name = "Имя", DataType = DataType.String, Description = "", IsBuiltIn = true },
            new Attributes { Id = 2, CategoryId = 1, Name = "Фамилия", DataType = DataType.String, Description = "", IsBuiltIn = true },
            new Attributes { Id = 3, CategoryId = 1, Name = "Дата рождения", DataType = DataType.Date, Description = "", IsBuiltIn = true },
            new Attributes { Id = 4, CategoryId = 1, Name = "Телефон", DataType = DataType.String, Description = "", IsBuiltIn = true },
            new Attributes { Id = 5, CategoryId = 1, Name = "Локация", DataType = DataType.String, Description = "", IsBuiltIn = true },
            new Attributes { Id = 6, CategoryId = 1, Name = "Фото профиля", DataType = DataType.Image, Description = "", IsBuiltIn = true },
            new Attributes { Id = 7, CategoryId = 1, Name = "О себе", DataType = DataType.Text, Description = "", IsBuiltIn = true }
        );
    }
    
    public virtual DbSet<Attributes> Attributes { get; set; }
    public virtual DbSet<AttributeValueOption> AttributeValueOptions { get; set; }
    public virtual DbSet<UserAttributes> UserAttributes { get; set; }
    public virtual DbSet<RecentlyUsedAttribute> RecentlyUsedAttribute { get; set; }
    public virtual DbSet<AttributeCategory> AttributeCategories { get; set; }
    public virtual DbSet<Position> Positions { get; set; }
    public virtual DbSet<PositionAttribute> PositionAttributes { get; set; }
    public virtual DbSet<PositionProjectTag> PositionProjectTags { get; set; }
    public virtual DbSet<AccessRule> AccessRules { get; set; }
    public virtual DbSet<Projects> Projects { get; set; }
    public virtual DbSet<ProjectTags> ProjectTags { get; set; }
}