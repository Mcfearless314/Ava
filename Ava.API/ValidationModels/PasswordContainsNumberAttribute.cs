using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ava.API.ValidationModels;

public class PasswordContainsNumberAttribute : ValidationAttribute
{
  protected override ValidationResult? IsValid(object? password, ValidationContext validationContext)
  {
    if (password is string)
    {
      if((password as string).Any(char.IsDigit)){
        return ValidationResult.Success;
      }
    }

    return new ValidationResult("Password must contain at least one number.");
  }
}
