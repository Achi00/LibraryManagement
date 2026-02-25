using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Domain.Entity;

namespace LibraryManagement.Application.Interfaces.Repositories
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        Task<IEnumerable<BookResponse>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<IEnumerable<BookResponse>> SearchAsync(string? title, string? author, CancellationToken cancellationToken = default);
        Task<bool> IsAvailableAsync(int bookId, CancellationToken cancellationToken);
        Task<bool> ExistsByIsbnAsync(string isbn, CancellationToken cancellationToken);
        Task<Book?> GetForUpdateAsync(int id, CancellationToken cancellationToken);
    }
}
