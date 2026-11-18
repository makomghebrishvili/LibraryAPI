namespace LibraryAPI.Services.Interfaces;

using LibraryAPI.DTOs.Books;
using LibraryAPI.DTOs.Common;

public interface IBookService
{
    Task<PagedResult<BookDTO>> GetAllAsync(string? search, int? authorId, int? genreId, bool? availableOnly, int page, int pageSize);
    Task<BookDetailsDTO> GetByIdAsync(int id);
    Task<BookDetailsDTO> CreateAsync(CreateBookRequest request);
    Task<BookDetailsDTO> UpdateAsync(int id, CreateBookRequest request);
    Task DeleteAsync(int id);
}
