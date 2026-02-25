using LibraryManagement.Application.DTOs.BorrowRecord;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Domain.Entity;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.BorrowRecords
{
    public class BorrowRecordRepository : IBorrowRecordRepository
    {
        private readonly LibraryManagementContext _context;

        public BorrowRecordRepository(LibraryManagementContext context)
        {
            _context = context;
        }
        public void Add(BorrowRecord record)
        {
            _context.BorrowRecords.Add(record);
        }

        public async Task<IEnumerable<BorrowRecordResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.BorrowRecords.AsNoTracking().ProjectToType<BorrowRecordResponse>().ToListAsync(cancellationToken);
        }

        public async Task<BorrowRecord?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.BorrowRecords.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<BorrowRecordResponse>> GetOverdueAsync(CancellationToken cancellationToken)
        {
            return await _context.BorrowRecords
                .AsNoTracking()
                .Where(b => b.ReturnDate == null && b.DueDate < DateTime.UtcNow)
                .ProjectToType<BorrowRecordResponse>()
                .ToListAsync(cancellationToken);
        }

        public async Task<BorrowRecord?> GetForUpdateAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.BorrowRecords.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

    }
}
