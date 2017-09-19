using System;
using System.Linq;
using System.Reflection;
using Common;
using FluentValidation;
using Journalist.Extensions;
using Microsoft.EntityFrameworkCore.Internal;
using UserManagment;

namespace MIPTCore.Models.ModelValidators
{
    public class AbstractUserModelValidator<T> : AbstractValidator<T> where T : AbstractUserModel
    {
        public AbstractUserModelValidator()
        {   
            RuleFor(_ => _).NotNull();
            
            RuleFor(userModel => userModel.EmailAddress)
                .NotEmpty().EmailAddress()
                .WithMessage(p => $"'{nameof(p.EmailAddress)}' must be valid Email. {p.EmailAddress} is not");

            RuleFor(userModel => userModel.AlumniProfile)
                .SetValidator(new AlumniProfileModelValidator());
                
            RuleFor(userModel => userModel.FirstName)
                .NotEmpty()
                .Must(_ => UserValidationPredicateService.BeLengthStronglyBetween(_, 1, 30))
                .WithMessage(p => $"'{nameof(p.FirstName)}' should be between 1 and 30 characters long. {p.FirstName} is not");
            RuleFor(userModel => userModel.LastName)
                .NotEmpty()
                .Must(_ => UserValidationPredicateService.BeLengthStronglyBetween(_, 1, 30))
                .WithMessage(p => $"'{nameof(p.LastName)}' should be between 1 and 30 characters long. {p.LastName} is not");
        }
    }
        
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            Include(new AbstractUserModelValidator<UserModel>());
            
            RuleFor(userModel => userModel.Id).GreaterThan(0);
            RuleFor(userModel => userModel.CreatingDate);
        }
    }

    public class UserRegistrationModelValidator : AbstractValidator<UserRegistrationModel>
    {
        public UserRegistrationModelValidator()
        {
            Include(new AbstractUserModelValidator<UserRegistrationModel>());

            RuleFor(userModel => userModel.Password).Must(Password.IsStringCorrectPassword)
                .WithMessage(p => $"'{nameof(p.Password)}' is not corresponding security rules. It should be between 8 and 16 characters");
        }
    }

    public class UserUpdateModelValidator : AbstractValidator<UserUpdateModel>
    {
        public UserUpdateModelValidator()
        {
            Include(new AbstractUserModelValidator<UserUpdateModel>());
        }
    }

    internal static class UserValidationPredicateService
    {
        public static bool BeLengthStronglyBetween(string strinToTest, int min, int max)
        {
            return strinToTest != null && !strinToTest.Equals(string.Empty) && strinToTest.Length > min &&
                   strinToTest.Length < max;
        }
    }
}