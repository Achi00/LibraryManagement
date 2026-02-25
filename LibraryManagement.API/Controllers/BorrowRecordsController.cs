using LibraryManagement.Application.DTOs.BorrowRecord;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Tags("Borrow Records")]
    public class BorrowRecordsController : ControllerBase
    {
        private readonly IBorrowRecordService _service;

        public BorrowRecordsController(IBorrowRecordService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all borrow records
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of all borrow records</returns>
        /// <response code="200">Returns the list of borrow records</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowRecordResponse>>> GetAll(
            CancellationToken cancellationToken = default)
        {
            return Ok(await _service.GetAllAsync(cancellationToken));
        }

        /// <summary>
        /// Get a specific borrow record by ID
        /// </summary>
        /// <param name="id">Borrow record ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Borrow record details</returns>
        /// <response code="200">Returns the borrow record</response>
        /// <response code="404">Borrow record not found</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowRecordResponse>> GetById(
            int id,
            CancellationToken cancellationToken = default)
        {
            return Ok(await _service.GetByIdAsync(id, cancellationToken));
        }

        [HttpPost]
        public async Task<ActionResult<BorrowRecordResponse>> Borrow(
            CreateBorrowRecordRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await _service.BorrowAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Return a borrowed book
        /// </summary>
        /// <param name="id">Borrow record ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>No content</returns>
        /// <response code="204">Book returned successfully</response>
        /// <response code="400">Book already returned</response>
        /// <response code="404">Borrow record not found</response>
        [HttpPut("{id}/return")]
        public async Task<IActionResult> Return(int id, CancellationToken cancellationToken = default)
        {
            await _service.ReturnAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<BorrowRecordResponse>>> GetOverdue(
            CancellationToken cancellationToken = default)
        {
            return Ok(await _service.GetOverdueAsync(cancellationToken));
        }
    }
}
