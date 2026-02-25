using LibraryManagement.Application.DTOs.BorrowRecord;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Domain.Entity;
using Mapster;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Services
{
    public class BorrowRecordService : IBorrowRecordService
    {
        private readonly IBorrowRecordRepository _borrowRecordRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IPatronRepository _patronRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BorrowRecordService> _logger;


        public BorrowRecordService(IBorrowRecordRepository borrowRecordRepository, IBookRepository bookRepository, IPatronRepository patronRepository, IUnitOfWork unitOfWork, ILogger<BorrowRecordService> logger)
        {
            _borrowRecordRepository = borrowRecordRepository;
            _bookRepository = bookRepository;
            _patronRepository = patronRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<BorrowRecordResponse> BorrowAsync(CreateBorrowRecordRequest dto, CancellationToken cancellationToken)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var book = await _bookRepository.GetForUpdateAsync(dto.BookId, cancellationToken);

                if (book == null)
                {
                    throw new NotFoundException($"Book with id {dto.BookId} was not found");
                }

                if (book.Quantity <= 0)
                {
                    throw new BusinessRuleViolationException("Book is not available for borrow");
                }

                var patron = await _patronRepository.GetByIdAsync(dto.PatronId, cancellationToken);

                if (patron == null)
                {
                    throw new NotFoundException($"Patron with id {dto.PatronId} was not found");
                }


                book.Quantity--;

                var record = BorrowRecord.Create(dto.BookId, dto.PatronId);

                _borrowRecordRepository.Add(record);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Patron {PatronId} is borrowing book {BookId}",
                    dto.PatronId,
                    dto.BookId);


                return record.Adapt<BorrowRecordResponse>();
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<IEnumerable<BorrowRecordResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _borrowRecordRepository.GetAllAsync(cancellationToken);
        }

        public async Task<BorrowRecordResponse> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var record = await _borrowRecordRepository
                .GetByIdAsync(id, cancellationToken);

            if (record == null)
            {
                throw new NotFoundException($"Borrow record with id {id} not found");
            }

            return record.Adapt<BorrowRecordResponse>();
        }


        public async Task<IEnumerable<BorrowRecordResponse>> GetOverdueAsync(CancellationToken cancellationToken)
        {
            return await _borrowRecordRepository.GetOverdueAsync(cancellationToken);
        }

        public async Task ReturnAsync(int id, CancellationToken cancellationToken)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var record = await _borrowRecordRepository.GetForUpdateAsync(id, cancellationToken);

                if (record == null)
                {
                    throw new NotFoundException($"Borrow with id {id} was not found");
                }

                if (record.ReturnDate != null)
                {
                    throw new BusinessRuleViolationException("Book already returned");
                }

                record.MarkReturned();

                var book = await _bookRepository.GetForUpdateAsync(record.BookId, cancellationToken);
                if (book == null)
                {
                    throw new NotFoundException($"Book with id {record.BookId} was not found");
                }
                book.Quantity++;

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Borrow record {RecordId} returned",
                    id);


                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
