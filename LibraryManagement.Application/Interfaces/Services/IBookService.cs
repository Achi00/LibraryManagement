using LibraryManagement.Application.DTOs.Book;

namespace LibraryManagement.Application.Interfaces.Services
{
    public interface IBookService
    {
        Task<BookResponse> AddBookAsync(CreateBookRequest dto, CancellationToken cancellationToken);
        Task<IEnumerable<BookResponse>> GetAllBooksAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<BookResponse> GetBookByIdAsync(int id, CancellationToken cancellationToken);
        Task UpdateBookAsync(int id, UpdateBookRequest dto, CancellationToken cancellationToken);
        Task<IEnumerable<BookResponse>> SearchBookAsync(string title, string author, CancellationToken cancellationToken);
        Task DeleteBookAsync(int id, CancellationToken cancellationToken);
    }
}
