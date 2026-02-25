using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.DTOs.Patron;

namespace LibraryManagement.Application.Interfaces.Services
{
    public interface IPatronService
    {
        Task<IEnumerable<PatronResponse>> GetAllAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken);

        Task<PatronResponse> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<PatronResponse> CreatePatronAsync(
            CreatePatronRequest request,
            CancellationToken cancellationToken);

        Task UpdatePatronAsync(
            int id,
            UpdatePatronRequest dto,
            CancellationToken cancellationToken);

        Task DeletePatronAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<BookResponse>> GetBorrowedBooksAsync(
            int patronId,
            CancellationToken cancellationToken);
    }
}
