using FluentValidation;
using LibraryManagement.Application.DTOs.Book;

namespace LibraryManagement.Application.Validators.Book
{
    public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.ISBN)
            .NotEmpty()
            .Length(10, 13);

            RuleFor(x => x.PublicationYear)
                .NotEmpty()
                .InclusiveBetween(1000, DateTime.UtcNow.Year)
                .WithMessage("Book's publication year must be between 1000 and Current year");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.AuthorId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.Description)
                .MaximumLength(500);

            RuleFor(x => x.CoverImageUrl);
        }
    }
}
