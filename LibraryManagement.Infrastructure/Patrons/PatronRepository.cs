using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.DTOs.Patron;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Domain.Entity;
using LibraryManagement.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Patrons
{
    public class PatronRepository : IPatronRepository
    {
        private readonly LibraryManagementContext _context;

        public PatronRepository(LibraryManagementContext context)
        {
            _context = context;
        }
        public void Add(Patron entity)
        {
            _context.Patrons.Add(entity);
        }

        public void Delete(Patron entity)
        {
            _context.Patrons.Remove(entity);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var patron = await _context.Patrons.AsNoTracking().Where(p => p.Email == email).ToListAsync(cancellationToken);

            if (patron == null)
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<PatronResponse>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _context.Patrons.AsNoTracking().ProjectToType<PatronResponse>().ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BookResponse>> GetBorrowedBooksAsync(int patronId, CancellationToken cancellationToken = default)
        {
            return await _context.BorrowRecords
                .AsNoTracking()
                .Where(b => b.PatronId == patronId && b.ReturnDate == null)
                .Select(b => b.Book)
                .ProjectToType<BookResponse>()
                .ToListAsync(cancellationToken);
        }

        public async Task<Patron?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Patrons.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Patron?> GetForUpdateAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Patrons.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<bool> HasActiveBorrowsAsync(int patronId, CancellationToken cancellationToken)
        {
            return await _context.BorrowRecords.AnyAsync(br => br.PatronId == patronId && br.ReturnDate == null, cancellationToken);
        }

    }
}
