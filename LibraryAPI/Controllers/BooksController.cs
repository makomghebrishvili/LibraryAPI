namespace LibraryAPI.Controllers;

using LibraryAPI.DTOs.Books;
using LibraryAPI.DTOs.Common;
using LibraryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/books")]
[Authorize]

public class BooksController:ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<BookDTO
        >>> GetAll(
        [FromQuery] string? search,
        [FromQuery] int? authorId,
        [FromQuery] int? genreId,
        [FromQuery] bool? availableOnly,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        return Ok(await _bookService.GetAllAsync(search, authorId, genreId, availableOnly, page, pageSize));
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<BookDetailsDTO>> GetById(int id)
    {
        return Ok(await _bookService.GetByIdAsync(id));
    }
    [HttpPost]
    [Authorize(Roles = "Librarian")]
    public async Task<ActionResult<BookDetailsDTO>> Create(CreateBookRequest request)
    {
        var created = await _bookService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "Librarian")]
    public async Task<ActionResult<BookDetailsDTO>> Update(int id, CreateBookRequest request)
    {
        return Ok(await _bookService.UpdateAsync(id, request));
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> Delete(int id)
    {
        await _bookService.DeleteAsync(id);
        return NoContent();
    }
}
