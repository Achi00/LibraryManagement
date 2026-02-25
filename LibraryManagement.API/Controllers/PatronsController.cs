using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.DTOs.Patron;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Tags("Patrons")]
    public class PatronsController : ControllerBase
    {
        private readonly IPatronService _patronService;

        public PatronsController(IPatronService patronService)
        {
            _patronService = patronService;
        }

        // <summary>
        /// Get all patrons with pagination
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Number of items per page (default: 10)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Paginated list of patrons</returns>
        /// <response code="200">Returns the paginated list of patrons</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatronResponse>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            var patrons = await _patronService.GetAllAsync(page, pageSize, ct);
            return Ok(patrons);
        }

        /// <summary>
        /// Get a specific patron by ID
        /// </summary>
        /// <param name="id">Patron ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Patron details</returns>
        /// <response code="200">Returns the patron</response>
        /// <response code="404">Patron not found</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<PatronResponse>> GetById(
            int id,
            CancellationToken ct)
        {
            var patron = await _patronService.GetByIdAsync(id, ct);
            return Ok(patron);
        }

        /// <summary>
        /// Get all currently borrowed books by a patron
        /// </summary>
        /// <param name="id">Patron ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of borrowed books</returns>
        /// <response code="200">Returns the list of borrowed books</response>
        /// <response code="404">Patron not found</response>
        [HttpGet("{id}/books")]
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetBorrowedBooks(
            int id,
            CancellationToken ct)
        {
            var books = await _patronService.GetBorrowedBooksAsync(id, ct);
            return Ok(books);
        }

        /// <summary>
        /// Create a new patron
        /// </summary>
        /// <param name="request">Patron creation details</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Created patron</returns>
        /// <response code="201">Patron created successfully</response>
        /// <response code="400">Invalid request data</response>
        [HttpPost]
        public async Task<ActionResult<PatronResponse>> Create(
            [FromBody] CreatePatronRequest request,
            CancellationToken ct)
        {
            var result = await _patronService.CreatePatronAsync(request, ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        /// <summary>
        /// Update an existing patron
        /// </summary>
        /// <param name="id">Patron ID</param>
        /// <param name="request">Updated patron details</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>No content</returns>
        /// <response code="204">Patron updated successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="404">Patron not found</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdatePatronRequest request,
            CancellationToken ct)
        {
            await _patronService.UpdatePatronAsync(id, request, ct);
            return NoContent();
        }

        /// <summary>
        /// Delete a patron
        /// </summary>
        /// <param name="id">Patron ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>No content</returns>
        /// <response code="204">Patron deleted successfully</response>
        /// <response code="404">Patron not found</response>
        /// <response code="400">Cannot delete patron with active borrows</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken ct)
        {
            await _patronService.DeletePatronAsync(id, ct);
            return NoContent();
        }
    }
}
