namespace LibraryAPI.DTOs.Books;

public class BookDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublishedYear { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public string AuthorFullName { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = new();

}
