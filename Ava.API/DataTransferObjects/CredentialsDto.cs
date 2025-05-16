using System.ComponentModel.DataAnnotations;
using Ava.API.ValidationModels;

namespace Ava.API.DataTransferObjects;

public class CredentialsDto
{
  public required string Username { get; set; }
  [MinLength(8)] [PasswordContainsNumber] [PasswordContainsSpecialCharacter] [PasswordContainsUppercaseLetter] public required string Password { get; set; }
}
