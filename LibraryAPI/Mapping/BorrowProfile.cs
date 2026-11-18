namespace LibraryAPI.Mapping;

using AutoMapper;
using LibraryAPI.DTOs.Borrows;
using LibraryAPI.Entities;

public class BorrowProfile:Profile
{
    public BorrowProfile()
    {
        CreateMap<BorrowRecord, BorrowRecordDTO>()
            .ForMember(d => d.UserFullName, o => o.MapFrom(s => s.User.FullName))
            .ForMember(d => d.BookTitle, o => o.MapFrom(s => s.Book.Title));
    }
}
