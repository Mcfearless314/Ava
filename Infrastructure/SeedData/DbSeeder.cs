using Infrastructure;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.SeedData;

public static class DbSeeder
{
  public static void Seed(IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Database.Migrate();

    if (!context.Organisations.Any())
    {
      var organisation = new Organisation
      {
        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        Name = "TestCorp",
      };

      context.Organisations.Add(organisation);
    }

    if (!context.Users.Any())
    {
      var users = new List<User>
      {
        new()
        {
          Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
          Username = "Alice",
          Credentials = new Credentials
          {
            UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            PasswordHash = "kafkakfkafkamfkamfkamf",
            Salt = "kadmakmdkamdkamdkamdkamd"
          },
          CreatedAt = DateTime.Now
        },
        new()
        {
          Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
          Username = "Bob",
          Credentials = new Credentials
          {
            UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            PasswordHash = "kafkakfkafkamfkamfkamf",
            Salt = "kadmakmdkamdkamdkamdkamd"
          },
          CreatedAt = DateTime.Now
        },
        new()
        {
          Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
          Username = "Charlie",
          Credentials = new Credentials
          {
            UserId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            PasswordHash = "kafkakfkafkamfkamfkamf",
            Salt = "kadmakmdkamdkamdkamdkamd"
          },
          CreatedAt = DateTime.Now
        },
        new()
        {
          Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
          Username = "Dana",
          Credentials = new Credentials
          {
            UserId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            PasswordHash = "kafkakfkafkamfkamfkamf",
            Salt = "kadmakmdkamdkamdkamdkamd"
          },
          CreatedAt = DateTime.Now
        },
        new()
        {
          Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
          Username = "Eve",
          Credentials = new Credentials
          {
            UserId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
            PasswordHash = "kafkakfkafkamfkamfkamf",
            Salt = "kadmakmdkamdkamdkamdkamd"
          },
          CreatedAt = DateTime.Now
        },
      };

      context.Users.AddRange(users);
    }

    if (!context.Projects.Any())
    {
      var project = new Project
      {
        Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
        Title = "Test Project",
        Subtitle = "This is a test project.",
        OrganisationId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        ProjectManagerId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
      };

      context.Projects.Add(project);
    }

    if (!context.ProjectUsers.Any())
    {
      var projectUsers = new List<ProjectUser>
      {
        new()
        {
          ProjectId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
          UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
        },
        new()
        {
          ProjectId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
          UserId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
        },
      };

      context.ProjectUsers.AddRange(projectUsers);
    }


    context.SaveChanges();
  }
}
