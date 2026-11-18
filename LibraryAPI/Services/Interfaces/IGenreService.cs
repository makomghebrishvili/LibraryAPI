namespace LibraryAPI.Services.Interfaces;

using LibraryAPI.DTOs.Genres;

public interface IGenreService
{
    Task<List<GenreDTO>> GetAllAsync();
    Task<GenreDTO> CreateAsync(CreateGenreRequest request);
}
