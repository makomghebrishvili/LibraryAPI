namespace LibraryAPI.DTOs.LibraryCards;

public class CreateLibraryCardRequest
{
    public int UserId { get; set; }
    public int ValidYears { get; set; } = 1;
}
