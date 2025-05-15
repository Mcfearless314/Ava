using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration;

public static class Dependencies
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
  {
    services.AddScoped<IOrganizationRepository, OrganisationRepository>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IProjectRepository, ProjectRepository>();

    services.AddDbContext<AppDbContext>(options => { options.UseSqlite(connectionString); });
    return services;
  }
}
