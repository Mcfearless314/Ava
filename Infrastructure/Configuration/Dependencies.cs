using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration;

public static class Dependencies
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
  {
    services.AddDbContext<AppDbContext>(options => { options.UseSqlite(connectionString); });

    return services;
  }
}
