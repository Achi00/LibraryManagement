namespace LibraryManagement.Application.DTOs.Book
{
    public class UpdateBookRequest
    {
        public string Title { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public int PublicationYear { get; set; }
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public int Quantity { get; set; }
        public int AuthorId { get; set; }
    }

}
