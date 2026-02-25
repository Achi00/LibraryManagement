using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.DTOs.Patron;
using LibraryManagement.Domain.Entity;

namespace LibraryManagement.Application.Interfaces.Repositories
{
    public interface IPatronRepository
    {
        Task<Patron?> GetByIdAsync(int id, CancellationToken ct);
        Task<IEnumerable<PatronResponse>> GetAllAsync(
            int page,
            int pageSize,
            CancellationToken ct);

        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);

        Task<bool> HasActiveBorrowsAsync(int patronId, CancellationToken ct);

        void Add(Patron patron);
        void Delete(Patron patron);

        Task<Patron?> GetForUpdateAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<BookResponse>> GetBorrowedBooksAsync(
            int patronId,
            CancellationToken ct);
    }

}
