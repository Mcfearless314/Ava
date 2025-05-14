using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration;

public static class Dependencies
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
  {
    services.AddScoped<IUserRepository, UserRepository>();

    services.AddDbContext<AppDbContext>(options => { options.UseSqlite(connectionString); });
    return services;
  }
}
