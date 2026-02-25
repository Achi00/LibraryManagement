using LibraryManagement.Application.DTOs.BorrowRecord;

namespace LibraryManagement.Application.Interfaces.Services
{
    public interface IBorrowRecordService
    {
        Task<BorrowRecordResponse> BorrowAsync(
            CreateBorrowRecordRequest request,
            CancellationToken cancellationToken);

        Task ReturnAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<BorrowRecordResponse>> GetAllAsync(CancellationToken cancellationToken);

        Task<BorrowRecordResponse> GetByIdAsync(int id, CancellationToken ct);
        Task<IEnumerable<BorrowRecordResponse>> GetOverdueAsync(CancellationToken cancellationToken);

    }

}
