using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Domain.Entity;
using LibraryManagement.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Books
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryManagementContext _context;
        public BookRepository(LibraryManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookResponse>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _context.Books
                .AsNoTracking()
                .ProjectToType<BookResponse>()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public void Add(Book entity)
        {
            _context.Books.Add(entity);
        }

        public void Delete(Book entity)
        {
            _context.Books.Remove(entity);
        }

        public async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> IsAvailableAsync(int bookId, CancellationToken cancellationToken)
        {
            return await _context.Books.AsNoTracking().AnyAsync(b => b.Id == bookId && b.Quantity > 0, cancellationToken);
        }

        public async Task<IEnumerable<BookResponse>> SearchAsync(string? title, string? author, CancellationToken cancellationToken = default)
        {
            var query = _context.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(b => b.Title.Contains(title));
            }

            if (!string.IsNullOrWhiteSpace(author))
            {
                query = query.Where(b =>
                    (b.Author.FirstName + " " + b.Author.LastName)
                    .Contains(author));
            }

            return await query.ProjectToType<BookResponse>().ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByIsbnAsync(string isbn, CancellationToken cancellationToken)
        {
            return await _context.Books.AnyAsync(b => b.ISBN == isbn, cancellationToken);
        }

        // used instead of get by id method because this allows sstate change, this method does not uses AsNoTracking
        public async Task<Book?> GetForUpdateAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }
    }
}
