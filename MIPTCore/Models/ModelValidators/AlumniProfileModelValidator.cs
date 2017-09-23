using System;
using FluentValidation;
using UserManagment;

namespace MIPTCore.Models.ModelValidators
{   
    public class AlumniProfileModelValidator : AbstractValidator<AlumniProfileModel>
    {
        public AlumniProfileModelValidator()
        {
            RuleFor(alumniProfileModel => alumniProfileModel.Diploma)
                .Must(m => Enum.IsDefined(typeof(DiplomaType), m))
                .WithMessage(m => $"'{nameof(m.Diploma)}' must be vaild dimploma code");
            
            RuleFor(alumniProfileModel => alumniProfileModel.Faculty)
                .Must(m => Enum.IsDefined(typeof(FacultyType), m))
                .WithMessage(m => $"'{nameof(m.Faculty)}' must be vaild faculty code");
            
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