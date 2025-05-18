using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IUserActionsLogRepository
{
  Task SaveLogAsync(UserActionsLog log);
}

