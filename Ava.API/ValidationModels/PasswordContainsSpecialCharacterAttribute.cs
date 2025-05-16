using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ava.API.ValidationModels;

public class PasswordContainsSpecialCharacterAttribute : ValidationAttribute
{
  protected override ValidationResult? IsValid(object? password, ValidationContext validationContext)
  {
    if (password is string)
    {
      if((password as string).Any(c => !char.IsLetterOrDigit(c))){
        return ValidationResult.Success;
      }
    }

    return new ValidationResult("Password must contain at least one special character. Valid special characters include: ! @ # $ % ^ & * ( ) _ + - = { } [ ] | \\\\ : ; , . ? /");
  }
}
