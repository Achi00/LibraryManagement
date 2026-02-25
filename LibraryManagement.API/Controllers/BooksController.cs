using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Tags("Books")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Get all books with pagination
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Number of items per page (default: 10)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of books</returns>
        /// <response code="200">Returns the paginated list of books</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BookResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var books = await _bookService.GetAllBooksAsync(page, pageSize, cancellationToken);
            
            return Ok(books);
        }

        /// <summary>
        /// Get a specific book by ID
        /// </summary>
        /// <param name="id">Book ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Book details</returns>
        /// <response code="200">Returns the book</response>
        /// <response code="404">Book not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookResponse>> GetById(int id, CancellationToken cancellationToken = default)
        {
            var book = await _bookService.GetBookByIdAsync(id, cancellationToken);

            return Ok(book);
        }

        /// <summary>
        /// Search books by title or author name
        /// </summary>
        /// <param name="title">Book title (optional)</param>
        /// <param name="author">Author name (optional)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of matching books</returns>
        /// <response code="200">Returns matching books</response>
        /// <response code="400">At least one search parameter is required</response>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookResponse>>> SearchBook([FromQuery] string title, [FromQuery] string author, CancellationToken cancellationToken = default)
        {
            var book = await _bookService.SearchBookAsync(title, author, cancellationToken);

            return Ok(book);
        }

        /// <summary>
        /// Create a new book
        /// </summary>
        /// <param name="request">Book creation details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created book</returns>
        /// <response code="201">Book created successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="404">Author not found</response>
        [HttpPost]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookResponse>> CreateBook(CreateBookRequest request, CancellationToken cancellationToken = default)
        {
            var createdBook = await _bookService.AddBookAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = createdBook.Id }, createdBook);
        }

        /// <summary>
        /// Update an existing book
        /// </summary>
        /// <param name="id">Book ID</param>
        /// <param name="request">Updated book details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>No content</returns>
        /// <response code="204">Book updated successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="404">Book or Author not found</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateBookRequest request,
            CancellationToken cancellationToken = default)
        {
            await _bookService.UpdateBookAsync(id, request, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Delete a book
        /// </summary>
        /// <param name="id">Book ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>No content</returns>
        /// <response code="204">Book deleted successfully</response>
        /// <response code="404">Book not found</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken = default)
        {
            await _bookService.DeleteBookAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
