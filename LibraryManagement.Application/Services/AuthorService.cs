using LibraryManagement.Application.DTOs.Author;
using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Domain.Entity;
using Mapster;

namespace LibraryManagement.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AuthorService(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
        {
            _authorRepository = authorRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<AuthorResponse> AddAuthorAsync(CreateAuthorRequest dto, CancellationToken cancellationToken)
        {
            var author = dto.Adapt<Author>();

            _authorRepository.Add(author);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return author.Adapt<AuthorResponse>();
        }

        public async Task DeleteAuthorAsync(int id, CancellationToken cancellationToken)
        {
            if (id < 0)
            {
                throw new ArgumentException("Author id can't be negative value");
            }

            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                throw new NotFoundException("Author was not found");
            }

            var bookCount = author.Books.Count();

            if (bookCount > 0)
            {
                throw new BusinessRuleViolationException($"Author with id {id} can not be deleted because they have associated books.");
            }

            _authorRepository.Delete(author);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<BookResponse>> GetAllBooksByAuthorIdAsync(int id, CancellationToken cancellationToken)
        {
            if (id < 0)
            {
                throw new ArgumentException("Author id can't be negative value");
            }

            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                throw new NotFoundException($"Author not found");
            }

            return await _authorRepository.GetAllBooksByAuthorId(id);
        }

        public async Task<IEnumerable<AuthorResponse>> GetAllAuthorsAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            if (page < 0)
            {
                throw new ArgumentException("Page must be positive value");
            }
            if (pageSize <= 0)
            {
                throw new ArgumentException("PageSize must be 0 or more");
            }

            return await _authorRepository.GetAllAsync(page, pageSize, cancellationToken);
        }

        public async Task<AuthorResponse> GetAuthorByIdAsync(int id, CancellationToken cancellationToken)
        {
            if (id < 0)
            {
                throw new ArgumentException("Author id can't be negative value");
            }
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                throw new NotFoundException("Author was not found");
            }

            return author.Adapt<AuthorResponse>();
        }

        public async Task UpdateAuthorAsync(int id, UpdateAuthorRequest dto, CancellationToken cancellationToken)
        {
            if (id < 0)
            {
                throw new ArgumentException("Author id can't be negative value");
            }

            var author = await _authorRepository.GetForUpdateAsync(id, cancellationToken);

            if (author == null)
            {
                throw new NotFoundException("Author was not found");
            }

            dto.Adapt(author);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
