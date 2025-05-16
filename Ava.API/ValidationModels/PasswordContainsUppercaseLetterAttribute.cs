using System.ComponentModel.DataAnnotations;

namespace Ava.API.ValidationModels;

public class PasswordContainsUppercaseLetterAttribute : ValidationAttribute
{
  protected override ValidationResult? IsValid(object? password, ValidationContext validationContext)
  {
    if (password is string)
    {
      if((password as string).Any(char.IsUpper)){
        return ValidationResult.Success;
      }
    }

    return new ValidationResult("Password must contain at least one uppercase letter.");
  }
}
