namespace LibraryAPI.DTOs.Books;

using LibraryAPI.DTOs.Authors;
using LibraryAPI.DTOs.Genres;

public class BookDetailsDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublishedYear { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public AuthorDTO Author { get; set; } = null!;
    public List<GenreDTO> Genres { get; set; } = new();
}
