using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileServiceApi.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace FileServiceApi.Models.Validations
{
    public class RegisterUserValidation : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidation(UserServiceDbContext dbContext)
        {

            RuleFor(c => c.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(c => c.Password)
                .MinimumLength(8);

            RuleFor(c => c.ConfirmedPassword).Equal(x => x.Password);

            RuleFor(c => c.Email).Custom(
                (value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(c => c.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", $"{value} in use!");
                    }
                });
        }
    }
}
