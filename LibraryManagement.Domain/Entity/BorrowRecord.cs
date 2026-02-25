using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entity
{
    public class BorrowRecord
    {
        public int Id { get; private set; }

        public int BookId { get; private set; }
        public Book Book { get; private set; } = null!;

        public int PatronId { get; private set; }
        public Patron Patron { get; private set; } = null!;

        public DateTime BorrowDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? ReturnDate { get; private set; }

        public BorrowStatus Status { get; private set; } 

        private BorrowRecord() { }

        private BorrowRecord(int bookId, int patronId, int loanDays)
        {
            var now = DateTime.UtcNow;

            BookId = bookId;
            PatronId = patronId;
            BorrowDate = now;
            DueDate = now.AddDays(loanDays);
            Status = BorrowStatus.Borrowed;
        }
        public static BorrowRecord Create(int bookId, int patronId, int loanDays = 14)
        {
            return new BorrowRecord(bookId, patronId, loanDays);
        }

        public void MarkReturned()
        {
            ReturnDate = DateTime.UtcNow;
            Status = BorrowStatus.Returned;
        }

        public void MarkOverdue()
        {
            if (Status == BorrowStatus.Borrowed && DueDate < DateTime.UtcNow)
            {
                Status = BorrowStatus.Overdue;
            }
        }

        public void UpdateOverdueStatus()
        {
            if (Status == BorrowStatus.Borrowed && DueDate < DateTime.UtcNow)
            {
                Status = BorrowStatus.Overdue;
            }
        }
    }
}
