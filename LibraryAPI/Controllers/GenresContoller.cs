namespace LibraryAPI.Controllers;

using LibraryAPI.DTOs.Genres;
using LibraryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/genres")]
[Authorize]

public class GenresController:ControllerBase
{
    private readonly IGenreService _genreService;
    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }
    [HttpGet]
    public async Task<ActionResult<List<GenreDTO>>> GetAll()
    {
        return Ok(await _genreService.GetAllAsync());

    }
    [HttpPost]
    [Authorize(Roles = "Librarian")]
    public async Task<ActionResult<GenreDTO>> Create(CreateGenreRequest request)
    {
        var created = await _genreService.CreateAsync(request);
        return Created("", created);
    }
}
