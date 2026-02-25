using LibraryManagement.Application.DTOs.Author;
using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Tags("Authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        /// <summary>
        /// Get all authors with pagination
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Number of items per page (default: 10)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of authors</returns>
        /// <response code="200">Returns the paginated list of authors</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorResponse>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var authors = await _authorService.GetAllAuthorsAsync(page, pageSize, cancellationToken);

            return Ok(authors);
        }

        /// <summary>
        /// Get a specific author by ID
        /// </summary>
        /// <param name="id">Author ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Author details</returns>
        /// <response code="200">Returns the author</response>
        /// <response code="404">Author not found</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorResponse>> GetById(int id, CancellationToken cancellationToken = default)
        {
            var author = await _authorService.GetAuthorByIdAsync(id, cancellationToken);

            return Ok(author);
        }

        /// <summary>
        /// Get all books by a specific author
        /// </summary>
        /// <param name="id">Author ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of books by the author</returns>
        /// <response code="200">Returns the list of books</response>
        /// <response code="404">Author not found</response>
        [HttpGet("{id}/books")]
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetAllBooksByAuthorId(int id, CancellationToken cancellationToken = default)
        {
            var books = await _authorService.GetAllBooksByAuthorIdAsync(id, cancellationToken);

            return Ok(books);
        }

        /// <summary>
        /// Create a new author
        /// </summary>
        /// <param name="dto">Author creation details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created author</returns>
        /// <response code="201">Author created successfully</response>
        /// <response code="400">Invalid request data</response>
        [HttpPost]
        public async Task<ActionResult<AuthorResponse>> CreateAuthor(CreateAuthorRequest dto, CancellationToken cancellationToken = default)
        {
            var newAuthor = await _authorService.AddAuthorAsync(dto, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = newAuthor.Id }, newAuthor);
        }

        /// <summary>
        /// Update an existing author
        /// </summary>
        /// <param name="id">Author ID</param>
        /// <param name="dto">Updated author details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>No content</returns>
        /// <response code="204">Author updated successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="404">Author not found</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuthor(int id, UpdateAuthorRequest dto, CancellationToken cancellationToken = default)
        {
            await _authorService.UpdateAuthorAsync(id, dto, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Delete an author
        /// </summary>
        /// <param name="id">Author ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>No content</returns>
        /// <response code="204">Author deleted successfully</response>
        /// <response code="404">Author not found</response>
        /// <response code="400">Cannot delete author with existing books</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuthor(int id, CancellationToken cancellationToken = default)
        {
            await _authorService.DeleteAuthorAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
