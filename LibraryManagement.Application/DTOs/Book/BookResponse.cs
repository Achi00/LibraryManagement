namespace LibraryManagement.Application.DTOs.Book
{
    public class BookResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public int PublicationYear { get; set; }
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable => Quantity > 0;

        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
