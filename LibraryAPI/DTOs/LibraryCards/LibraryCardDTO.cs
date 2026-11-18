namespace LibraryAPI.DTOs.LibraryCards;

public class LibraryCardDTO
{
    public int Id { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public bool IsExpired => ExpiresAt < DateTime.UtcNow;
}
