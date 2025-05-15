namespace Service.Services.Utility;

public static class GenerateRandomProjectTaskId
{
  public static string GenerateRandomId(int length = 4)
  {
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    var random = new Random();
    return new string(Enumerable.Range(0, length)
      .Select(_ => chars[random.Next(chars.Length)]).ToArray());
  }
}
