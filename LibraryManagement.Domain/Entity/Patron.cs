namespace LibraryManagement.Domain.Entity
{
    public class Patron
    {
        public int Id { get; private set; }

        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public DateTime MembershipDate { get; private set; }

        public ICollection<BorrowRecord> BorrowRecords { get; private set; } = new List<BorrowRecord>();

    }
}
