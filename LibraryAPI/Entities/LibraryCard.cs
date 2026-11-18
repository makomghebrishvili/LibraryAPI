namespace LibraryAPI.Entities;

public class LibraryCard
{
    public int Id { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;

}
