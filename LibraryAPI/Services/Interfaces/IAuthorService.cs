namespace LibraryAPI.Services.Interfaces;

using LibraryAPI.DTOs.Authors;
using LibraryAPI.DTOs.Common;

public interface IAuthorService
{
    Task<PagedResult<AuthorDTO>> GetAllAsync(string? search, int page, int pageSize);
    Task<AuthorDetailsDTO> GetByIdAsync(int id);
    Task<AuthorDTO> CreateAsync(CreateAuthorRequest request);
    Task<AuthorDTO> UpdateAsync(int id, CreateAuthorRequest request);
    Task DeleteAsync(int id);
}
