namespace LibraryManagement.Domain.Entity
{
    public class Book
    {
        public int Id { get; private set; }

        public string Title { get; private set; } = null!;
        public string ISBN { get; private set; } = null!;
        public int PublicationYear { get; private set; }
        public string? Description { get; private set; }
        public string? CoverImageUrl { get; private set; }
        public int Quantity { get; set; }

        public int AuthorId { get; private set; }
        // docs says Book -> AuthorId, 1 book has one author and not many!!!
        public Author Author { get; private set; } = null!;

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
