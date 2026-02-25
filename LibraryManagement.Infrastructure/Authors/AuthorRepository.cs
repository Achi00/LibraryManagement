using LibraryManagement.Application.DTOs.Author;
using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Domain.Entity;
using LibraryManagement.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Authors
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryManagementContext _context;

        public AuthorRepository(LibraryManagementContext context)
        {
            _context = context;
        }

        public void Add(Author entity)
        {
            _context.Authors.Add(entity);
        }

        public void Delete(Author entity)
        {
            _context.Authors.Remove(entity);
        }

        public async Task<IEnumerable<AuthorResponse>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .AsNoTracking()
                .ProjectToType<AuthorResponse>()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BookResponse>> GetAllBooksByAuthorId(int authorId, CancellationToken cancellationToken = default)
        {
            return await _context.Books.AsNoTracking().Where(b => b.AuthorId == authorId).ProjectToType<BookResponse>().ToListAsync(cancellationToken);
        }

        public async Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Authors.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<Author?> GetForUpdateAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Authors.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }
    }
}
