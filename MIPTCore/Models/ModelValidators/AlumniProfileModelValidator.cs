using System;
using FluentValidation;
using UserManagment;

namespace MIPTCore.Models.ModelValidators
{   
    public class AlumniProfileModelValidator : AbstractValidator<AlumniProfileModel>
    {
        public AlumniProfileModelValidator()
        {
            RuleFor(alumniProfileModel => alumniProfileModel.Faculty)
                .NotEmpty();
            
            RuleFor(alumniProfileModel => alumniProfileModel.YearOfGraduation).Must(BePositive)
                .Must(m => m <= DateTime.Now.Year)
                .WithMessage(m => $"'{nameof(m.YearOfGraduation)}' must be in past");
        }   
        
        private bool BePositive(int param)
        {
            return param > 0;
        }
    }
}