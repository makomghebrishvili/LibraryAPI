namespace LibraryAPI.Mapping;

using AutoMapper;
using LibraryAPI.DTOs.Books;
using LibraryAPI.Entities;

public class BookProfile:Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookDTO>()
           .ForMember(x => x.AuthorFullName, y => y.MapFrom(s => $"{s.Author.FirstName} {s.Author.LastName}"))
           .ForMember(x => x.Genres, y => y.MapFrom(s => s.Genres.Select(g => g.Name).ToList()));
        CreateMap<Book, BookDetailsDTO>();
        CreateMap<CreateBookRequest, Book>()
            .ForMember(x => x.Id, y => y.Ignore())
            .ForMember(x=> x.AvailableCopies, y => y.Ignore())
            .ForMember(x => x.Author, y=> y.Ignore())
            .ForMember(x=> x.Genres, y => y.Ignore())
            .ForMember(x => x.BorrowRecords, y => y.Ignore());


    }
}
