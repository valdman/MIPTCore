using Common;
using FluentValidation;

namespace MIPTCore.Models.ModelValidators
{
    public class CredentialsValidator : AbstractValidator<Credentials>
    {
        public CredentialsValidator()
        {
            RuleFor(credentials => credentials.Email).NotEmpty().EmailAddress()
                .WithMessage(p => $"'{nameof(p.Email)}' must be valid Email. {p.Email} is not");
            
            RuleFor(credentials => credentials.Password).Must(Password.IsStringCorrectPassword)
                .WithMessage(p => $"'{nameof(p.Password)}' is not corresponding security rules. It should be between 8 and 16 characters");
        }
    }
}