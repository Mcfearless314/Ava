namespace Ava.API.CustomExceptions;

public class UserIsPartOfAnotherOrganisationException : Exception
{
  public UserIsPartOfAnotherOrganisationException()
    : base("User is already part of another organisation.") { }

  public UserIsPartOfAnotherOrganisationException(string message)
    : base(message) { }

  public UserIsPartOfAnotherOrganisationException(string message, Exception innerException)
    : base(message, innerException) { }
}
