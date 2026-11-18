namespace LibraryAPI.Controllers;

using LibraryAPI.DTOs.Authors;
using LibraryAPI.DTOs.Common;
using LibraryAPI.Services;
using LibraryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/authors")]
[Authorize]
public class AuthorsController:ControllerBase
{
    private readonly IAuthorService _authorService;
    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }
    [HttpGet]
    public async Task<ActionResult<PagedResult<AuthorDTO>>>GetAll(
        [FromQuery] string? search,
        [FromQuery]int page=1,
        [FromQuery]int pageSize=20
        )
    {
        return Ok(await _authorService.GetAllAsync(search, page, pageSize));
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorDetailsDTO>> GetById(int id)
    {
        return Ok(await _authorService.GetByIdAsync(id));
    }
    [HttpPost]
    [Authorize(Roles = "Librarian")]
    public async Task<ActionResult<AuthorDTO>> Create(CreateAuthorRequest request)
    {
        var created = await _authorService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
    [HttpPut("{id}")]
    [Authorize(Roles ="Librarian")]
    public async Task<ActionResult<AuthorDTO>> Update(int id, CreateAuthorRequest request)
    {
        return Ok(await _authorService.UpdateAsync(id, request));
    }
    [HttpDelete("{id}")]
    [Authorize(Roles ="Librarian")]
    public async Task<IActionResult> Delete(int id)
    {
        await _authorService.DeleteAsync(id);
        return NoContent();
    }


}
