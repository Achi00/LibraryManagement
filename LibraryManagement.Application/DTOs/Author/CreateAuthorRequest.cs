namespace LibraryManagement.Application.DTOs.Author
{
    public class CreateAuthorRequest
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Biography { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }

}
