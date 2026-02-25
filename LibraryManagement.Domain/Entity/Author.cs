namespace LibraryManagement.Domain.Entity
{
    public class Author
    {
        public int Id { get; private set; }

        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public string? Biography { get; private set; }
        public DateTime? DateOfBirth { get; private set; }

        public ICollection<Book> Books { get; private set; } = new List<Book>();
    }
}
