using System;
using System.Text.RegularExpressions;
using Common;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;

namespace MIPTCore.Models.ModelValidators
{
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleSet("UserBaseRuleset", () =>
            {
                RuleFor(_ => _).NotNull();
                RuleFor(userModel => userModel.FirstName).NotEmpty();
                RuleFor(userModel => userModel.LastName).NotEmpty();
                RuleFor(userModel => userModel.Email).Must(UserValidationPredicateService.BeValidEmail);
            });

            RuleFor(userModel => userModel.Id).GreaterThan(0);
            RuleFor(userModel => userModel.FirstName)
                .Must(_ => UserValidationPredicateService.BeLengthStronglyBetween(_, 1, 30));
            RuleFor(userModel => userModel.LastName)
                .Must(_ => UserValidationPredicateService.BeLengthStronglyBetween(_, 1, 30));

            RuleFor(userModel => userModel.AlumniProfile).NotNull().When(user => user.IsMiptAlumni);
            RuleFor(userModel => userModel.CreatingDate);
        }
    }

    public class UserRegistrationModelValidator : AbstractValidator<UserRegistrationModel>
    {
        public UserRegistrationModelValidator()
        {
            RuleSet("UserBaseRuleset", () =>
            {
                RuleFor(_ => _).NotNull();
                RuleFor(userModel => userModel.FirstName).NotEmpty();
                RuleFor(userModel => userModel.LastName).NotEmpty();
                RuleFor(userModel => userModel.Email).Must(UserValidationPredicateService.BeValidEmail);
            });

            RuleFor(userModel => userModel.FirstName)
                .Must(_ => UserValidationPredicateService.BeLengthStronglyBetween(_, 1, 30));
            RuleFor(userModel => userModel.LastName)
                .Must(_ => UserValidationPredicateService.BeLengthStronglyBetween(_, 1, 30));
            RuleFor(userModel => userModel.Password).Must(Password.IsStringCorrectPassword);

            RuleFor(userModel => userModel.AlumniProfile).NotNull().When(user => user.IsMiptAlumni);
        }
    }

    public class UserUpdateModelValidator : AbstractValidator<UserUpdateModel>
    {
        public UserUpdateModelValidator()
        {
            RuleSet("UserBaseRuleset", () =>
            {
                RuleFor(_ => _).NotNull();
                RuleFor(userModel => userModel.FirstName).NotEmpty();
                RuleFor(userModel => userModel.LastName).NotEmpty();
                RuleFor(userModel => userModel.Email).Must(UserValidationPredicateService.BeValidEmail);
            });

            RuleFor(userModel => userModel.FirstName)
                .Must(_ => UserValidationPredicateService.BeLengthStronglyBetween(_, 1, 30));
            RuleFor(userModel => userModel.LastName)
                .Must(_ => UserValidationPredicateService.BeLengthStronglyBetween(_, 1, 30));

            RuleFor(userModel => userModel.AlumniProfile).NotNull().When(user => user.IsMiptAlumni);
        }
    }

    internal static class UserValidationPredicateService
    {
        public static bool BeLengthStronglyBetween(string strinToTest, int min, int max)
        {
            return strinToTest != null && !strinToTest.Equals(string.Empty) && strinToTest.Length > min &&
                   strinToTest.Length < max;
        }

        public static bool BeValidEmail(string email)
        {
            var regex = new Regex(
                "^((([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+(\\.([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+)*)|((\\x22)((((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(([\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x7f]|\\x21|[\\x23-\\x5b]|[\\x5d-\\x7e]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(\\\\([\\x01-\\x09\\x0b\\x0c\\x0d-\\x7f]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF]))))*(((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(\\x22)))@((([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-||_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.)+(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+|(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+([a-z]+|\\d|-|\\.{0,1}|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])?([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))$",
                RegexOptions.IgnoreCase);
            return email != null && regex.IsMatch(email);
        }
    }
}