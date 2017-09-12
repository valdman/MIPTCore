using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Xunit;

namespace IntegrationTestProject
{
    public class ValidatorTests
    {
        internal class TestObject
        {
            public TestObject(string val)
            {
                this.EmailScarecrow = val;
            }

            public string EmailScarecrow { get; set; }
        }
        
        internal class TestValidator : InlineValidator<TestObject>
        {
            public TestValidator (params Action<TestValidator >[] actions)
            {
                foreach (var action in actions)
                {
                    action(this);
                }
            }
        }

        [Fact]
        public void EmailValidatorFromFluenValidationLibraryWokrs()
        {
            //Arrange
            var validator = new TestValidator(v => v.RuleFor(obj => obj.EmailScarecrow).SetValidator( new EmailValidator() ));
            
            //Act, Assert
            Assert.True(validator.Validate(new TestObject("boris@mail.ru"))        .IsValid);
            Assert.True(validator.Validate(new TestObject("boris.valdman@mail.ru")).IsValid);
            Assert.True(validator.Validate(new TestObject("q@mail.ru"))            .IsValid);
            Assert.True(validator.Validate(new TestObject("bo1is@ma1l.ru"))        .IsValid);
            
            Assert.False(validator.Validate(new TestObject(" @mail.ru"))            .IsValid);
            Assert.False(validator.Validate(new TestObject("borismail.ru"))         .IsValid);
            Assert.False(validator.Validate(new TestObject("boris@mailru"))         .IsValid);
            Assert.False(validator.Validate(new TestObject(""))                     .IsValid);
            
            //False 'true' on null
        }
    }
}