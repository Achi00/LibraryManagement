using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.DTOs.Patron;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Domain.Entity;
using Mapster;

namespace LibraryManagement.Application.Services
{
    public class PatronService : IPatronService
    {
        private readonly IPatronRepository _patronRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PatronService(IPatronRepository patronRepository, IUnitOfWork unitOfWork)
        {
            _patronRepository = patronRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PatronResponse> CreatePatronAsync(CreatePatronRequest dto, CancellationToken cancellationToken)
        {
            var exists = await _patronRepository.ExistsByEmailAsync(dto.Email, cancellationToken);
            if (exists)
            {
                throw new AlreadyExistsException($"Patron with email {dto.Email} already exists");
            }

            var patron = dto.Adapt<Patron>();

            _patronRepository.Add(patron);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return patron.Adapt<PatronResponse>();
        }

        public async Task DeletePatronAsync(int id, CancellationToken cancellationToken)
        {
            if (id < 0)
            {
                throw new ArgumentException("Patron id can't be negative value");
            }
            var patron = await _patronRepository.GetByIdAsync(id, cancellationToken);


            if (patron == null)
            {
                throw new NotFoundException($"Patron with Id {id} was not found");
            }

            var hasActiveBorrows = await _patronRepository.HasActiveBorrowsAsync(id, cancellationToken);

            if (hasActiveBorrows)
            {
                throw new BusinessRuleViolationException("Patron has active borrow records and cannot be deleted");
            }

            _patronRepository.Delete(patron);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<PatronResponse>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _patronRepository.GetAllAsync(page, pageSize, cancellationToken);
        }

        public async Task<IEnumerable<BookResponse>> GetBorrowedBooksAsync(int patronId, CancellationToken cancellationToken)
        {
            if (patronId < 0)
            {
                throw new ArgumentException("PatronId can't be negative value");
            }

            return await _patronRepository.GetBorrowedBooksAsync(patronId, cancellationToken);
        }

        public async Task<PatronResponse> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var patron = await _patronRepository.GetByIdAsync(id, cancellationToken);
            return patron.Adapt<PatronResponse>();
        }

        public async Task UpdatePatronAsync(int id, UpdatePatronRequest dto, CancellationToken cancellationToken)
        {
            if (id < 0)
            {
                throw new ArgumentException("Author id can't be negative value");
            }

            var patron = await _patronRepository.GetForUpdateAsync(id, cancellationToken);

            if (patron == null)
            {
                throw new NotFoundException($"Patron with id {id} was not found");
            }

            dto.Adapt(patron);

            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
