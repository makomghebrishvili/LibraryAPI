namespace LibraryAPI.Services;

using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.DTOs.Books;
using LibraryAPI.DTOs.Common;
using LibraryAPI.Entities;
using LibraryAPI.Exceptions;
using LibraryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class BookService:IBookService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public BookService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<PagedResult<BookDTO>> GetAllAsync(
        string? search, int? authorId, int? genreId, bool? availableOnly, int page, int pageSize)
    {
        var query = _db.Books
            .Include(b => b.Author)
            .Include(b => b.Genres)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower();
            query = query.Where(b => b.Title.ToLower().Contains(s) || b.ISBN.Contains(s));
        }
        if (authorId.HasValue)
            query = query.Where(b => b.AuthorId == authorId.Value);

        if (genreId.HasValue)
            query = query.Where(b => b.Genres.Any(g => g.Id == genreId.Value));

        if (availableOnly == true)
            query = query.Where(b => b.AvailableCopies > 0);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(b => b.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return new PagedResult<BookDTO>
        {
            Items = _mapper.Map<List<BookDTO>>(items),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
    public async Task<BookDetailsDTO> GetByIdAsync(int id)
    {
        var book = await _db.Books
            .Include(b => b.Author)
            .Include(b => b.Genres)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
            throw new NotFoundException($"Book with ID {id} not found.");

        return _mapper.Map<BookDetailsDTO>(book);
    }
    public async Task<BookDetailsDTO> CreateAsync(CreateBookRequest request)
    {
        if (await _db.Books.AnyAsync(b => b.ISBN == request.ISBN))
            throw new ConflictException($"Book with ISBN '{request.ISBN}' already exists.");

        var author = await _db.Authors.FindAsync(request.AuthorId);
        if (author == null)
            throw new NotFoundException($"Author with ID {request.AuthorId} not found.");

        var distinctGenreIds = request.GenreIds.Distinct().ToList();
        var genres = await _db.Genres.Where(g => distinctGenreIds.Contains(g.Id)).ToListAsync();
        if (genres.Count != distinctGenreIds.Count)
            throw new NotFoundException("One or more specified genres were not found.");

        var book = _mapper.Map<Book>(request);
        book.AvailableCopies = request.TotalCopies;
        book.Genres = genres;
        _db.Books.Add(book);
        await _db.SaveChangesAsync();

        return await GetByIdAsync(book.Id);
    }

    public async Task<BookDetailsDTO> UpdateAsync(int id, CreateBookRequest request)
    {
        var book = await _db.Books
            .Include(b => b.Genres)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
            throw new NotFoundException($"Book with ID {id} not found.");

        if (await _db.Books.AnyAsync(b => b.ISBN == request.ISBN && b.Id != id))
            throw new ConflictException($"Book with ISBN '{request.ISBN}' already exists.");

        if (!await _db.Authors.AnyAsync(a => a.Id == request.AuthorId))
            throw new NotFoundException($"Author with ID {request.AuthorId} not found.");

        var distinctGenreIds = request.GenreIds.Distinct().ToList();
        var genres = await _db.Genres.Where(g => distinctGenreIds.Contains(g.Id)).ToListAsync();
        if (genres.Count != distinctGenreIds.Count)
            throw new NotFoundException("One or more specified genres were not found.");

        int borrowedCount = book.TotalCopies - book.AvailableCopies;
        if (request.TotalCopies < borrowedCount)
            throw new BusinessRuleException($"TotalCopies cannot be less than currently borrowed copies ({borrowedCount}).");

        _mapper.Map(request, book);
        book.AvailableCopies = request.TotalCopies - borrowedCount;
        book.Genres.Clear();
        foreach (var g in genres)
            book.Genres.Add(g);

        await _db.SaveChangesAsync();
        return await GetByIdAsync(book.Id);
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _db.Books
            .Include(b => b.BorrowRecords)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
            throw new NotFoundException($"Book with ID {id} not found.");

        if (book.BorrowRecords.Any(r => r.ReturnedAt == null))
            throw new BusinessRuleException("Cannot delete book that is currently borrowed.");

        _db.Books.Remove(book);
        await _db.SaveChangesAsync();
    }
}
