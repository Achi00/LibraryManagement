using LibraryManagement.Application.DTOs.BorrowRecord;
using LibraryManagement.Domain.Entity;

namespace LibraryManagement.Application.Interfaces.Repositories
{
    public interface IBorrowRecordRepository
    {
        Task<BorrowRecord?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<IEnumerable<BorrowRecordResponse>> GetAllAsync(CancellationToken cancellationToken);

        Task<IEnumerable<BorrowRecordResponse>> GetOverdueAsync(CancellationToken cancellationToken);
        Task<BorrowRecord?> GetForUpdateAsync(int id, CancellationToken ct);

        void Add(BorrowRecord record);
    }

}
