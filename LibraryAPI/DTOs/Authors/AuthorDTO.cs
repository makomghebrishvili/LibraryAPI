namespace LibraryAPI.DTOs.Authors;

public class AuthorDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int BirthYear { get; set; }
    public string Country { get; set; } = string.Empty;
}
