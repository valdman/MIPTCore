using FluentValidation;

namespace MIPTCore.Models.ModelValidators
{
    public class AbstractCapitalModelValidator<T> : AbstractValidator<T> where T : AbstractCapitalModel
    {
        public AbstractCapitalModelValidator()
        {
            RuleFor(c => c.Description).NotEmpty()
                .WithMessage(c => $"'{nameof(c.Description)}' should not be empty");
            RuleFor(c => c.Description).Must(description => description?.Length < 240)
                .WithMessage(c => $"'{nameof(c.Description)}' should not be shorther than 120 characters");

            RuleFor(c => c.Name).NotEmpty()
                .WithMessage(c => $"'{nameof(c.Name)}' should not be empty");
            RuleFor(c => c.Name).Must(name => name?.Length < 120)
                .WithMessage(c => $"'{nameof(c.Name)}' should not be shorther than 60 characters");
        }
    }

    public class CapitalCreatingmodelValidator : AbstractValidator<CapitalCreatingModel>
    {
        public CapitalCreatingmodelValidator()
        {
            Include(new AbstractCapitalModelValidator<CapitalCreatingModel>());
        }
    }

    public class CapitalUpdatingModelvalidator : AbstractValidator<CapitalUpdatingModel>
    {
        public CapitalUpdatingModelvalidator()
        {
            Include(new AbstractCapitalModelValidator<CapitalUpdatingModel>());
        }
    }

    public class CapitalModelValidator : AbstractValidator<CapitalModel>
    {
        public CapitalModelValidator()
        {
            Include(new AbstractCapitalModelValidator<CapitalModel>());
        }
    }
}