using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.Repositories;

public class UserActionsLogRepository : IUserActionsLogRepository
{
  private readonly AppDbContext _context;

  public UserActionsLogRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task SaveLogAsync(UserActionsLog log)
  {
    _context.UserActionsLogs.Add(log);
    await _context.SaveChangesAsync();
  }
}
