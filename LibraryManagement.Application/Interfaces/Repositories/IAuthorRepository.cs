using LibraryManagement.Application.DTOs.Author;
using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Domain.Entity;

namespace LibraryManagement.Application.Interfaces.Repositories
{
    public interface IAuthorRepository : IBaseRepository<Author>
    {
        Task<IEnumerable<AuthorResponse>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<IEnumerable<BookResponse>> GetAllBooksByAuthorId(int authorId, CancellationToken cancellationToken = default);
        Task<Author?> GetForUpdateAsync(int id, CancellationToken cancellationToken);
    }
}
