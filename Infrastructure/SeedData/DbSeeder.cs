using System.Reflection.Metadata;
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

    var users = new List<User>();

    if (!context.Users.Any())
    {
      var usersWithoutOrganisation = new List<User>
      {
        new()
        {
          Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
          Username = "Alice",
          Credentials = new Credentials
          {
            UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            PasswordHash =
              "zwDwLi8a97FHEhF6lGjwAQsrta0r3pTZzhKWf8i7Y+hpV2ni5TSX7LjQ7gmkYIFu5/hzcYcoRdtSVZzUvoo+IIJJ6JAZNkus7oBwycAEL+eB6TKVPH1IlnnSMb+mp+NguJqzlDoNij2h3Wn6nR2lJVXpbx+V8NWVAxa4BNmMURw=",
            Salt = "x+3pm8HUy8xyt5FzPmbLIrhOWT9Y16D6+q0owfVb9kk="
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
              "BjvFBVeAxMYwGAlQqMZwQ56iRd3jVYDVYYAZFxPy6RdAGqhDGTNj53KpbX7nxZCZPVvaS85ooCr7V9bfDYaVvbSX0GeF4Rhx53drEilv5+aWbF0f9BNX2orYuQIpjlOr9JT+j13WST5icU3/3Pk1Q3MMljnXNdmR0/PYgYLXDMU=",
            Salt = "RIjQO/9KMRQ667Q5cw4NLnMOgHnDptburNimhrJ2ksk="
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
              "1e/Wtrzamlo5UQAAtnWV3GzKqZ2DgcxX5P9Yzp8A2kWnaRkH7kpTswplWR1h46ZRvaJEqoEMi8LsAtCvlaiTTtOhK82j34UAd2IXwOPfkY2QtxmvfkErcOYVsM2CspTo4rabr/WmgVFBbVNZKrAo29I+moVNpk65A2B5Rw4ZbEU=",
            Salt = "UIIZ4kf+uhbGwlUtMlxDeYAJ98Ju/qoFiSyJA5CA4hU="
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
              "gjBjncme6+HJRGy1ja+E/1n2p7FJE5POR27I4NcKFuLZcTi0oB4GecanlDTc3DBT8WRQdpHNxJaF3MPzhB+cD35U3BMaKckrP+LSky/EoADZcfdEFaY396z9b0mv9KipYBzZhHbdtH+63XhjNwKDbhzIBXGolu3sHSFml056H5o=",
            Salt = "D90cCRK6Q9Q30V1ylC2t+0JiNPjXQYLIk7qhVAZ8ea4="
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
              "BrkK3hZ096lnYE5A/Wuxu0MNb9xDOzcpFCrAVUSU3SZMGuTvmDYkpX8snzxtRmQruLh5GqTexmnqak1Va78DxQip7gdW5YF0+GNWqY//515EAAIgvHTXZTkMwYB1a5T8QBn/EOhDWkKhdIqFbbD6SZZ+mD4M1f81kAKVZdpcJCE=",
            Salt = "z38n+5PyDOTyQT/WVQKf4LGQCKqi3tm7AMqU/nP+Ix8="
          },
          CreatedAt = DateTime.Now
        },
      };
      users.AddRange(usersWithoutOrganisation);
      context.Users.AddRange(users);
      context.SaveChanges();
    }

    if (!context.Organisations.Any())
    {
      var organisations = new List<Organisation>
      {
        new()
        {
          Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
          Name = "TestCorp",
          AdminUserId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
        },
        new()
        {
          Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
          Name = "AnotherCorp",
          AdminUserId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
        }
      };

      context.Organisations.AddRange(organisations);
      context.SaveChanges();
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
        },
        new()
        {
          Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
          Title = "Another Test Project",
          Subtitle = "This is another test project.",
          OrganisationId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        }
      };
      context.Projects.AddRange(projects);
      context.SaveChanges();
    }

    if (!context.ProjectUsers.Any())
    {
      var projectUsers = new List<ProjectUser>
      {
        new()
        {
          ProjectId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
          UserId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
          Role = ProjectRoles.ProjectManager,
        },
        new()
        {
          ProjectId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
          UserId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
          Role = ProjectRoles.ProjectManager,
        },
        new()
        {
          ProjectId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
          UserId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
          Role = ProjectRoles.Contributor,
        },
      };

      context.ProjectUsers.AddRange(projectUsers);
      context.SaveChanges();
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

      foreach (var user in users)
      {
        switch (user.Username)
        {
          case "Alice":
          case "Charlie":
            user.OrganisationId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            break;
          case "Bob":
          case "Dana":
          case "Eve":
            user.OrganisationId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            break;
        }
      }

      context.SaveChanges();
    }
  }
}
