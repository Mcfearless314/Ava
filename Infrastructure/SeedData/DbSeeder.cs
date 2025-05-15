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
      var organisations = new List<Organisation>
      {
        new()
        {
          Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
          Name = "TestCorp",
        },
        new()
        {
          Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
          Name = "AnotherCorp",
        }
      };

      context.Organisations.AddRange(organisations);
    }

    if (!context.Users.Any())
    {
      var users = new List<User>
      {
        new()
        {
          Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
          Username = "Alice",
          Credentials = new Credentials
          {
            UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            PasswordHash =
              "0/1L3XgpegjwB3RUOgA7wR116mDAt6Lxjr3Y71eYNvfXf8tMAvzfIRXy5Ozrv+AXMgibGwNDIMnbzIUh+TtC5CdLBfdeGesaRTAIGbBTQia9K3dvZEu0DNw61rerH3SmYJez6paNWrg+MUfvfsMaYoqTGcDxL0D9FXRtE3FDnjI=",
            Salt = "aUGTOIF3WKaQtbkX0hSRJA320sDmCN/fX5QG6+CfG/U="
          },
          CreatedAt = DateTime.Now
        },
        new()
        {
          Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
          Username = "Bob",
          Credentials = new Credentials
          {
            UserId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            PasswordHash =
              "1QKreXs2oVnhObBZow9ANtvRDiL+Y762kzJiCU5bR7/oSrkfvxfbxF1xPMU+E2oNTiOA/Ama6+q/GmqzMC7GePJ/3bb5TSFRNkFUcXUZCdI8LyQFZl7FpsSzhBLH+mn9injH2WXwP3t8BRWe5JPdVXRtKp8fpJ/KaVKz5724Yz8=",
            Salt = "xZSbFGUMiMm//8LgJ6kH73eHTZmvtOVxDH6PuDxy9ms="
          },
          CreatedAt = DateTime.Now
        },
        new()
        {
          Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
          Username = "Charlie",
          Credentials = new Credentials
          {
            UserId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            PasswordHash =
              "UeLCEv5D1yOwfHroNWClrk3rySq6Aza2UHWSns0T/B9v+C2u+IfWqAZVu1wM1vDrZo2gYbTK+FoZY7J8dDkiQQB0Y78RSgYTghdqUtaBtAVYuM7p5nSfUGvXeaaiMNrCLksOzdP/OKTl1+I1C4eSjp0vvVIUeaIJHTfE1H8JHwo=",
            Salt = "VJ675PzrQzTaN5kBB+IRmk8X4YiT7NCOPG4LR8o1uf8="
          },
          CreatedAt = DateTime.Now
        },
        new()
        {
          Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
          Username = "Dana",
          Credentials = new Credentials
          {
            UserId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
            PasswordHash =
              "PviyH1BT5/mDRQbtmZt5kQApZBrFyWlI5pfYa4iU2Em2ehQA6XB9PAkNxz2gHDeI+0aXcN6+ClO96nScniEayopiA14f8ZSWqtacvy2fXXwyuF1uI2Zrb//70GqIqqL/QkaB94uAM4enbXWMObFtAGgH8LjDppMfuJMT0YKkN78=",
            Salt = "fN/JlQVJv6XA9zHJ/31tQruv11IfBYatcYNd/pv3T1o="
          },
          CreatedAt = DateTime.Now
        },
        new()
        {
          Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
          Username = "Eve",
          Credentials = new Credentials
          {
            UserId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
            PasswordHash =
              "AtdUygsnuBfdDzMOLGQLhNIDThkolDJ8taAioUdU7sguknqLtHYoJazQR0lai6IWTmBMvOrz6IlyOt0iwwtADscyiuB7i6uIqJdk3O7sNN3Z2PAGEsJBqObnd3zu25UHpiYhF92gGyc7K4S0cp4ygpkoysX5j2mCZ5SuhAefVb0=",
            Salt = "rqxDEPswYXbRHiOW0/w2BcK9QssKyE4KQHO4Ii5qbpA="
          },
          CreatedAt = DateTime.Now
        },
      };

      context.Users.AddRange(users);
    }

    if (!context.Projects.Any())
    {
      var projects = new List<Project>()
      {
        new()
        {
          Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
          Title = "Test Project",
          Subtitle = "This is a test project.",
          OrganisationId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
          ProjectManagerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
        },
        new()
        {
          Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
          Title = "Another Test Project",
          Subtitle = "This is another test project.",
          OrganisationId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
          ProjectManagerId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
        }
      };
      context.Projects.AddRange(projects);
    }

    if (!context.ProjectUsers.Any())
    {
      var projectUsers = new List<ProjectUser>
      {
        new()
        {
          ProjectId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
          UserId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
        },
        new()
        {
          ProjectId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
          UserId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
        },
      };

      context.ProjectUsers.AddRange(projectUsers);
    }

    if (!context.UserRoles.Any())
    {
      var userRoles = new List<UserRole>
      {
        new UserRole
        {
          OrganisationId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
          UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
          Role = Roles.Admin
        },

        new UserRole
        {
          OrganisationId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
          UserId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
          Role = Roles.Admin
        },
        new UserRole
        {
          OrganisationId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
          UserId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
          Role = Roles.User
        },
        new UserRole
        {
          OrganisationId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
          UserId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
          Role = Roles.User
        },
        new UserRole
        {
          OrganisationId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
          UserId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
          Role = Roles.User
        }
      };

      context.UserRoles.AddRange(userRoles);
    }

    if (!context.ProjectTasks.Any())
    {
      var projectTasks = new List<ProjectTask>
      {
        new()
        {
          Id = "1111",
          Title = "Task 1",
          Body = "This is task 1.",
          CreatedAt = DateTime.Now,
          Status = ProjectTaskStatus.ToDo,
          ProjectId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
        },
        new()
        {
          Id = "2222",
          Title = "Task 2",
          Body = "This is task 2.",
          CreatedAt = DateTime.Now,
          Status = ProjectTaskStatus.InProgress,
          ProjectId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
        },
        new()
        {
          Id = "3333",
          Title = "Task 3",
          Body = "This is task 3.",
          CreatedAt = DateTime.Now,
          Status = ProjectTaskStatus.Done,
          ProjectId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
        },
        new()
        {
          Id = "4444",
          Title = "Task 4",
          Body = "This is task 4.",
          CreatedAt = DateTime.Now,
          Status = ProjectTaskStatus.Deleted,
          ProjectId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
        },
      };

      context.ProjectTasks.AddRange(projectTasks);
    }

    context.SaveChanges();
  }
}
