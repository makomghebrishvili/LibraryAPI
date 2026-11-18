namespace LibraryAPI.Services.Interfaces;

using LibraryAPI.DTOs.LibraryCards;

public interface ILibraryCardService
{
    Task<LibraryCardDTO> IssueCardAsync(CreateLibraryCardRequest request);
    Task<LibraryCardDTO> GetMyCardAsync(int userId);
    Task<LibraryCardDTO> GetByUserIdAsync(int userId);
    Task<LibraryCardDTO> RenewCardAsync(int userId, int years = 1);
}
