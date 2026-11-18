namespace LibraryAPI.Services;

using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.DTOs.Genres;
using LibraryAPI.Entities;
using LibraryAPI.Exceptions;
using LibraryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class GenreService:IGenreService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public GenreService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    public async Task<List<GenreDTO>> GetAllAsync()
    {
        var genres = await _db.Genres.OrderBy(g => g.Name).ToListAsync();
        return _mapper.Map<List<GenreDTO>>(genres);
    }
    public async Task<GenreDTO> CreateAsync(CreateGenreRequest request)
    {
        var exists = await _db.Genres.AnyAsync(g => g.Name.ToLower() == request.Name.ToLower());
        if (exists)
            throw new ConflictException("Genre with this name already exists.");

        var genre = _mapper.Map<Genre>(request);
        _db.Genres.Add(genre);
        await _db.SaveChangesAsync();

        return _mapper.Map<GenreDTO>(genre);
    }
}
