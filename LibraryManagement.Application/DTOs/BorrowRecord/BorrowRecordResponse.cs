using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.DTOs.BorrowRecord
{
    public class BorrowRecordResponse
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = null!;
        public string BookISBN { get; set; } = null!;
        public int PatronId { get; set; }
        public string PatronName { get; set; } = null!;
        public string PatronEmail { get; set; } = null!;
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public BorrowStatus Status { get; set; }
        public bool IsOverdue => Status == BorrowStatus.Borrowed && DateTime.UtcNow > DueDate;
        public int DaysOverdue => IsOverdue ? (DateTime.UtcNow - DueDate).Days : 0;
    }
}
