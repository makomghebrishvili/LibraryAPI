namespace LibraryAPI.DTOs.Borrows;

public class BorrowBookRequest
{
    public int BookId { get; set; }
    public int? UserId { get; set; }
    public int Days { get; set; } = 14;
}
