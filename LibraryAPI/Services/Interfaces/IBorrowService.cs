using LibraryAPI.DTOs.Borrows;
using LibraryAPI.DTOs.Common;

namespace LibraryAPI.Services.Interfaces;

public interface IBorrowService
{
    Task<BorrowRecordDTO> BorrowBookAsync(int currentUserId, string currentUserRole, BorrowBookRequest request);
    Task<BorrowRecordDTO> ReturnBookAsync(int borrowId, int currentUserId, string currentUserRole);
    Task<List<BorrowRecordDTO>> GetMyBorrowsAsync(int userId, bool activeOnly);
    Task<PagedResult<BorrowRecordDTO>> GetAllBorrowsAsync(bool? overdueOnly, int page, int pageSize);
}
