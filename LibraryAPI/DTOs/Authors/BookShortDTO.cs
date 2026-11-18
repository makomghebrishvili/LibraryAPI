namespace LibraryAPI.DTOs.Authors;

public class BookShortDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int PublishedYear { get; set; }
}
