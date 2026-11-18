namespace LibraryAPI.Services;

using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.DTOs.Authors;
using LibraryAPI.DTOs.Common;
using LibraryAPI.Entities;
using LibraryAPI.Exceptions;
using LibraryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class AuthorService : IAuthorService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public AuthorService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    public async Task<AuthorDetailsDTO> GetByIdAsync(int id)
    {
        var author = await _db.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (author == null)
            throw new NotFoundException($"Author with ID {id} not found.");

        return _mapper.Map<AuthorDetailsDTO>(author);
    }
    public async Task DeleteAsync(int id)
    {
        var author = await _db.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (author == null)
            throw new NotFoundException($"Author with ID {id} not found.");

        if (author.Books.Any())
            throw new BusinessRuleException("Cannot delete author who has existing books.");

        _db.Authors.Remove(author);
        await _db.SaveChangesAsync();
    }
    public async Task<AuthorDTO> UpdateAsync(int id, CreateAuthorRequest request)
    {
        var author = await _db.Authors.FindAsync(id);
        if (author == null)
            throw new NotFoundException($"Author with ID {id} not found.");

        _mapper.Map(request, author);
        await _db.SaveChangesAsync();

        return _mapper.Map<AuthorDTO>(author);
    }
    public async Task<AuthorDTO> CreateAsync(CreateAuthorRequest request)
    {
        var author = _mapper.Map<Author>(request);
        _db.Authors.Add(author);
        await _db.SaveChangesAsync();

        return _mapper.Map<AuthorDTO>(author);
    }
    public async Task<PagedResult<AuthorDTO>> GetAllAsync(string? search, int page, int pageSize)
    {
        var query = _db.Authors.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower();
            query = query.Where(a => a.FirstName.ToLower().Contains(s) || a.LastName.ToLower().Contains(s));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(a => a.LastName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return new PagedResult<AuthorDTO>
        {
            Items = _mapper.Map<List<AuthorDTO>>(items),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
}
