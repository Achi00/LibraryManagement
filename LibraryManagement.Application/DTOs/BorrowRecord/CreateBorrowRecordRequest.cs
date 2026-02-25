namespace LibraryManagement.Application.DTOs.BorrowRecord
{
    public class CreateBorrowRecordRequest
    {
        public int BookId { get; set; }
        public int PatronId { get; set; }
        public DateTime DueDate { get; set; }
    }

}
