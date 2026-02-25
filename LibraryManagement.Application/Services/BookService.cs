using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Domain.Entity;
using Mapster;

namespace LibraryManagement.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<BookResponse>> GetAllBooksAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            if (page < 0)
            {
                throw new ArgumentException("Page must be positive value");
            }
            if (pageSize <= 0)
            {
                throw new ArgumentException("PageSize must be 0 or more");
            }
            return await _bookRepository.GetAllAsync(page, pageSize, cancellationToken);
        }

        public async Task<BookResponse> GetBookByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id < 0)
            {
                throw new ArgumentException("Id should not be negative value");
            }

            var book = await _bookRepository.GetByIdAsync(id, cancellationToken);

            if (book == null)
            {
                throw new NotFoundException("Book was not found");
            }

            return book.Adapt<BookResponse>();
        }

        // add author validation!! if author id exists add book only then
        public async Task<BookResponse> AddBookAsync(CreateBookRequest dto, CancellationToken cancellationToken)
        {
            // check if author exists
            var author = await _authorRepository.GetByIdAsync(dto.AuthorId, cancellationToken);

            if (author == null)
            {
                throw new NotFoundException($"Author was not found, you can't add book with author id {dto.AuthorId}");
            }

            // check if ISBN already is in db
            var exists = await _bookRepository.ExistsByIsbnAsync(dto.ISBN, cancellationToken);

            if (exists)
            {
                throw new AlreadyExistsException("ISBN already exists");
            }

            var book = dto.Adapt<Book>();

            // create entity
            _bookRepository.Add(book);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return book.Adapt<BookResponse>();
        }

        public async Task UpdateBookAsync(int id, UpdateBookRequest dto, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetForUpdateAsync(id, cancellationToken);

            if (book == null)
            {
                throw new NotFoundException($"Book with id {id} was not found");
            }

            var author = await _authorRepository.GetByIdAsync(dto.AuthorId, cancellationToken);

            if (author == null)
            {
                throw new NotFoundException($"Author with id {dto.AuthorId} was not found");
            }

            book.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteBookAsync(int id, CancellationToken cancellationToken = default)
        {
            var book = await _bookRepository.GetForUpdateAsync(id, cancellationToken);

            if (book == null)
            {
                throw new NotFoundException("Book was not found");
            }

            _bookRepository.Delete(book);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<BookResponse>> SearchBookAsync(string title, string author, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(author))
            {
                throw new ArgumentException("At least one search parameter must be provided");
            }
            return await _bookRepository.SearchAsync(title, author, cancellationToken);
        }
    }
}
