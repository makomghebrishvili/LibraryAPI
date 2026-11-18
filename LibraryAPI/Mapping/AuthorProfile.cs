namespace LibraryAPI.Mapping;

using AutoMapper;
using LibraryAPI.DTOs.Authors;
using LibraryAPI.Entities;

public class AuthorProfile:Profile
{
    public AuthorProfile()
    {
        CreateMap<Author, AuthorDTO>();
        CreateMap<Book, BookShortDTO>();
        CreateMap<Author, AuthorDetailsDTO>().ForMember(x => x.Id, y => y.Ignore())
            .ForMember(x => x.Books, y => y.Ignore());
    }
}
