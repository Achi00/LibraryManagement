using FluentValidation;
using LibraryManagement.Application.DTOs.Patron;

namespace LibraryManagement.Application.Validators.Patron
{
    public class UpdatePatronRequestValidator : AbstractValidator<UpdatePatronRequest>
    {
        public UpdatePatronRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);
        }
    }

}
