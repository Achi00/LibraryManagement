using LibraryManagement.Application.DTOs.Author;
using LibraryManagement.Application.DTOs.Book;

namespace LibraryManagement.Application.Interfaces.Services
{
    public interface IAuthorService
    {
        Task<AuthorResponse> AddAuthorAsync(CreateAuthorRequest dto, CancellationToken cancellationToken);
        Task<AuthorResponse> GetAuthorByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<AuthorResponse>> GetAllAuthorsAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<IEnumerable<BookResponse>> GetAllBooksByAuthorIdAsync(int id, CancellationToken cancellationToken);
        Task UpdateAuthorAsync(int id, UpdateAuthorRequest dto, CancellationToken cancellationToken);
        Task DeleteAuthorAsync(int id, CancellationToken cancellationToken);
    }
}
