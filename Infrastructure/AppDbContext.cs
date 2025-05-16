using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;

namespace Infrastructure;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
  {
  }

  public DbSet<Credentials> Credentials { get; set; }
  public DbSet<Organisation> Organisations { get; set; }
  public DbSet<Project> Projects { get; set; }
  public DbSet<ProjectTask> ProjectTasks { get; set; }
  public DbSet<ProjectUser> ProjectUsers { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<UserActionsLog> UserActionsLogs { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Configuration for Credential entity
    modelBuilder.Entity<Credentials>(ent =>
    {
      ent.HasKey(c => c.UserId);

      ent.Property(c => c.PasswordHash)
        .IsRequired()
        .HasMaxLength(255);

      ent.Property(c => c.Salt)
        .IsRequired()
        .HasMaxLength(255);
    });

    // Configuration for Organisation entity
    modelBuilder.Entity<Organisation>(ent =>
    {
      ent.HasKey(o => o.Id);

      ent.Property(o => o.Name)
        .IsRequired()
        .HasMaxLength(100);

      ent.HasMany(o => o.Projects)
        .WithOne()
        .HasForeignKey(p => p.OrganisationId)
        .OnDelete(DeleteBehavior.Cascade);

      ent.HasOne(o => o.AdminUser)
        .WithOne()
        .HasForeignKey<Organisation>(o => o.AdminUserId)
        .OnDelete(DeleteBehavior.NoAction);

      ent.HasMany(o => o.Users)
        .WithOne()
        .HasForeignKey(u => u.OrganisationId)
        .OnDelete(DeleteBehavior.NoAction);
    });

    // Configuration for Project entity
    modelBuilder.Entity<Project>(ent =>
    {
      ent.HasKey(p => p.Id);

      ent.Property(p => p.Title).IsRequired().HasMaxLength(50);
      ent.Property(p => p.Subtitle).HasMaxLength(500);
      ent.Property(p => p.Id).IsRequired();

      ent.HasMany(p => p.Tasks)
        .WithOne()
        .HasForeignKey(t => t.ProjectId)
        .OnDelete(DeleteBehavior.Cascade);
    });


    // Configuration for ProjectTask entity
    modelBuilder.Entity<ProjectTask>(ent =>
    {
      ent.HasKey(pt => new { pt.ProjectId, pt.Id });

      ent.Property(pt => pt.Id)
        .IsRequired()
        .HasMaxLength(4);

      ent.HasOne(pt => pt.Project)
        .WithMany(p => p.Tasks)
        .HasForeignKey(pt => pt.ProjectId)
        .OnDelete(DeleteBehavior.Cascade);

      ent.Property(pt => pt.Status).IsRequired();
      ent.Property(pt => pt.CreatedAt).IsRequired();
      ent.Property(pt => pt.Title).IsRequired().HasMaxLength(50);
      ent.Property(pt => pt.Body).HasMaxLength(500);
    });

    // Configuration for ProjectUser entity
    modelBuilder.Entity<ProjectUser>(ent =>
    {
      ent.HasKey(pu => new { pu.ProjectId, pu.UserId });

      ent.Property(pu => pu.UserId).IsRequired();
      ent.Property(pu => pu.ProjectId).IsRequired();
      ent.Property(pu => pu.Role).IsRequired();
    });

    // Configuration for User entity
    modelBuilder.Entity<User>(ent =>
    {
      ent.HasKey(u => u.Id);

      ent.Property(u => u.Username)
        .IsRequired()
        .HasMaxLength(100);

      ent.Property(u => u.CreatedAt)
        .IsRequired();

      ent.HasOne(u => u.Credentials)
        .WithOne()
        .HasForeignKey<Credentials>(c => c.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    });

    // Configuration for UserActionsLog entity
    modelBuilder.Entity<UserActionsLog>(ent =>
    {
      ent.HasKey(ual => ual.ActionId);

      ent.Property(ual => ual.ActionLog).IsRequired();
      ent.Property(ual => ual.ActionPerformedAt).IsRequired();
      ent.Property(ual => ual.Description).IsRequired().HasMaxLength(500);
      ent.Property(ual => ual.ActionUser).IsRequired();


    });
  }
}
