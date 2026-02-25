using FluentValidation;
using LibraryManagement.Application.DTOs.BorrowRecord;

namespace LibraryManagement.Application.Validators
{
    public class CreateBorrowRecordRequestValidator
    : AbstractValidator<CreateBorrowRecordRequest>
    {
        public CreateBorrowRecordRequestValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0);

            RuleFor(x => x.PatronId)
                .GreaterThan(0);

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.UtcNow);
        }
    }

}
