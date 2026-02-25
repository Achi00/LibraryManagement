using FluentValidation;
using LibraryManagement.Application.DTOs.Author;

namespace LibraryManagement.Application.Validators
{
    public class CreateAuthorRequestValidator
    : AbstractValidator<CreateAuthorRequest>
    {
        public CreateAuthorRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Biography)
            .MaximumLength(2000);

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.UtcNow)
                .When(x => x.DateOfBirth.HasValue)
                .WithMessage("Date of Birth must be in past");
        }
    }

}
