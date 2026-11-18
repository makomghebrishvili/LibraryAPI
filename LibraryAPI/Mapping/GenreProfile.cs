namespace LibraryAPI.Mapping;

using AutoMapper;
using LibraryAPI.DTOs.Genres;
using LibraryAPI.Entities;

public class GenreProfile:Profile
{
    public GenreProfile()
    {
        CreateMap<Genre, GenreDTO>();
        CreateMap<CreateGenreRequest, Genre>().ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Books, o => o.Ignore());
    }
}
