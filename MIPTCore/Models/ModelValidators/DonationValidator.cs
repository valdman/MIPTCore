using FluentValidation;

namespace MIPTCore.Models.ModelValidators
{
    public class DonationValidator
    {
        public class AbstractDonationValidator<T> : AbstractValidator<T> where T : AbstractDonationModel
        {
            public AbstractDonationValidator()
            {   
                RuleFor(_ => _).NotNull();
            
                RuleFor(donationModel => donationModel.Value)
                    .GreaterThan(0).WithMessage(p => $"'{nameof(p.Value)}' must be valid positive. {p.Value} is not");
            }
        }

        public class DonationWithRegistrationModelValidator : AbstractValidator<DonationWithRegistrationModel>
        {
            public DonationWithRegistrationModelValidator()
            {
                Include(new AbstractDonationValidator<AbstractDonationModel>());
                
                RuleFor(donationModel => donationModel.CapitalId)
                    .GreaterThan(0).WithMessage(p => $"'{nameof(p.CapitalId)}' must be valid positive. {p.CapitalId} is not");
                
                RuleFor(userModel => userModel.FirstName)
                    .NotEmpty()
                    .Must(_ => UserValidationPredicateService.BeLengthStronglyBetween(_, 1, 30))
                    .WithMessage(p => $"'{nameof(p.FirstName)}' should be between 1 and 30 characters long. {p.FirstName} is not");
                RuleFor(userModel => userModel.LastName)
                    .NotEmpty()
                    .Must(_ => UserValidationPredicateService.BeLengthStronglyBetween(_, 1, 30))
                    .WithMessage(p => $"'{nameof(p.LastName)}' should be between 1 and 30 characters long. {p.LastName} is not");
                
                RuleFor(userModel => userModel.Email)
                    .NotEmpty().EmailAddress()
                    .WithMessage(p => $"'{nameof(p.Email)}' must be valid Email. {p.Email} is not");
                
                RuleFor(userModel => userModel.AlumniProfile)
                    .SetValidator(new AlumniProfileModelValidator());
            }
        }
        
        public class CreateDonationModelValidator : AbstractValidator<CreateDonationModel>
        {
            public CreateDonationModelValidator()
            {
                Include(new AbstractDonationValidator<AbstractDonationModel>());
                
                RuleFor(donationModel => donationModel.CapitalId)
                    .GreaterThan(0).WithMessage(p => $"'{nameof(p.CapitalId)}' must be valid positive. {p.CapitalId} is not");
                
                RuleFor(donationModel => donationModel.UserId)
                    .GreaterThan(0).WithMessage(p => $"'{nameof(p.UserId)}' must be valid positive. {p.UserId} is not");
            }
        }
        
        public class UpdateDonationModelValidator : AbstractValidator<UpdateDonationModel>
        {
            public UpdateDonationModelValidator()
            {
                Include(new AbstractDonationValidator<AbstractDonationModel>());
                
                RuleFor(donationModel => donationModel.CapitalId)
                    .GreaterThan(0).WithMessage(p => $"'{nameof(p.CapitalId)}' must be valid positive. {p.CapitalId} is not");
                
                RuleFor(donationModel => donationModel.UserId)
                    .GreaterThan(0).WithMessage(p => $"'{nameof(p.UserId)}' must be valid positive. {p.UserId} is not");
            }
        }
    }
}