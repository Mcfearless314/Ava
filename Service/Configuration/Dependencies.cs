using Microsoft.Extensions.DependencyInjection;
using Service.Services.Application;
using Service.Services.Security;

namespace Service.Configuration;

public static class Dependencies
{
  public static IServiceCollection AddService(this IServiceCollection services)
  {
    services.AddScoped<OrganisationService>();
    services.AddScoped<UserService>();
    services.AddScoped<ProjectService>();
    services.AddScoped<ProjectTaskService>();
    services.AddScoped<UserActionsLogService>();

    services.AddScoped<PasswordHashAlgorithm, Argon2IdPasswordHashAlgorithm>();
    return services;
  }
}
