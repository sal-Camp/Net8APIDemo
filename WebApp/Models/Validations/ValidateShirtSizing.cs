using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Validations;

public class ShirtEnsureCorrectSizingAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var shirt = validationContext.ObjectInstance as Shirt;

        if (shirt != null && !string.IsNullOrWhiteSpace(shirt.Gender))
        {
            if (shirt.Gender.Equals("men", StringComparison.OrdinalIgnoreCase) && shirt.Size < 8)
                return new ValidationResult("Men's shirts must have size greater or equal to 8.");

            if (shirt.Gender.Equals("women", StringComparison.OrdinalIgnoreCase) && shirt.Size < 6)
                return new ValidationResult("Women's shirts must have size greater or equal to 6.");
        }

        return ValidationResult.Success;
    }
}