namespace LibraryAPI.Services;

using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.DTOs.LibraryCards;
using LibraryAPI.Entities;
using LibraryAPI.Exceptions;
using LibraryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


public class LibraryCardService : ILibraryCardService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public LibraryCardService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<LibraryCardDTO> IssueCardAsync(CreateLibraryCardRequest request)
    {
        var user = await _db.Users
            .Include(u => u.LibraryCard)
            .FirstOrDefaultAsync(u => u.Id == request.UserId);

        if (user == null)
            throw new NotFoundException($"User with ID {request.UserId} not found.");

        if (user.LibraryCard != null)
            throw new ConflictException("This user already has a library card.");

        var nextId = (await _db.LibraryCards.MaxAsync(c => (int?)c.Id) ?? 0) + 1;
        var card = new LibraryCard
        {
            UserId = user.Id,
            CardNumber = $"LC-{nextId:D6}", // მაგ: LC-000001
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddYears(request.ValidYears)
        };

        _db.LibraryCards.Add(card);
        await _db.SaveChangesAsync();

        card.User = user;
        return _mapper.Map<LibraryCardDTO>(card);
    }

    public async Task<LibraryCardDTO> GetMyCardAsync(int userId)
    {
        return await GetByUserIdAsync(userId);
    }

    public async Task<LibraryCardDTO> GetByUserIdAsync(int userId)
    {
        var card = await _db.LibraryCards
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (card == null)
            throw new NotFoundException("Library card not found for this user.");

        return _mapper.Map<LibraryCardDTO>(card);
    }

    public async Task<LibraryCardDTO> RenewCardAsync(int userId, int years = 1)
    {
        var card = await _db.LibraryCards
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (card == null)
            throw new NotFoundException("Library card not found for this user.");

        var baseDate = card.ExpiresAt > DateTime.UtcNow ? card.ExpiresAt : DateTime.UtcNow;
        card.ExpiresAt = baseDate.AddYears(years);

        await _db.SaveChangesAsync();
        return _mapper.Map<LibraryCardDTO>(card);
    }
}

