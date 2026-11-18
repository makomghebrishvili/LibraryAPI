namespace LibraryAPI.Mapping;

using AutoMapper;
using LibraryAPI.DTOs.LibraryCards;
using LibraryAPI.Entities;

public class LibraryCardProfile:Profile
{
    public LibraryCardProfile()
    {
        CreateMap<LibraryCard, LibraryCardDTO>()
            .ForMember(d => d.UserFullName, o => o.MapFrom(s => s.User.FullName));
    }
}
