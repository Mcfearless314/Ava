using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infrastructure;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
  public AppDbContext CreateDbContext(string[] args)
  {
    var config = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddEnvironmentVariables(prefix: "AVA_")
      .Build();

    var connectionString = config.GetConnectionString("DBConnection");

    if (string.IsNullOrWhiteSpace(connectionString))
      throw new InvalidOperationException("DBConnection string not found in environment variables.");

    var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
    optionsBuilder.UseSqlite(connectionString);

    return new AppDbContext(optionsBuilder.Options);
  }
}
