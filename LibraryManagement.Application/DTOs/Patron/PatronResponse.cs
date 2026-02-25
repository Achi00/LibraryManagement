namespace LibraryManagement.Application.DTOs.Patron
{
    public class PatronResponse
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;

        public DateTime MembershipDate { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
