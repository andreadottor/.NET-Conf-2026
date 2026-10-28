using System.ComponentModel.DataAnnotations;
using Dottor.DotNetConf2026.Web.Services;

namespace Dottor.DotNetConf2026.Web.Validation;

public sealed class UniqueEmailAttribute : AsyncValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) =>
        throw new InvalidOperationException("UniqueEmail richiede la validazione asincrona nativa.");

    protected override async Task<ValidationResult?> IsValidAsync(object? value,
        ValidationContext validationContext, CancellationToken cancellationToken)
    {
        if (value is not string email || string.IsNullOrWhiteSpace(email) ||
            !new EmailAddressAttribute().IsValid(email))
        {
            return ValidationResult.Success;
        }

        var users = validationContext.GetRequiredService<UserDirectory>();
        return await users.EmailExistsAsync(email, cancellationToken)
            ? new ValidationResult("Email già registrata.")
            : ValidationResult.Success;
    }
}