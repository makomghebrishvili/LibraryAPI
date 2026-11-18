namespace LibraryAPI.Services;

using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.DTOs.Borrows;
using LibraryAPI.DTOs.Common;
using LibraryAPI.Entities;
using LibraryAPI.Exceptions;
using LibraryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class BorrowService : IBorrowService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public BorrowService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<BorrowRecordDTO> BorrowBookAsync(int currentUserId, string currentUserRole, BorrowBookRequest request)
    {
        int targetUserId = (currentUserRole == "Librarian" && request.UserId.HasValue)
            ? request.UserId.Value
            : currentUserId;

        var user = await _db.Users
            .Include(u => u.LibraryCard)
            .Include(u => u.BorrowRecords)
            .FirstOrDefaultAsync(u => u.Id == targetUserId);

        if (user == null)
            throw new NotFoundException($"User with ID {targetUserId} not found.");
        if (user.LibraryCard == null)
            throw new BusinessRuleException("User does not have a library card.");

        if (user.LibraryCard.ExpiresAt < DateTime.UtcNow)
            throw new BusinessRuleException("User's library card has expired.");

        var activeBorrows = user.BorrowRecords.Where(r => r.ReturnedAt == null).ToList();
        if (activeBorrows.Count >= 3)
            throw new BusinessRuleException("User cannot borrow more than 3 books at the same time.");

        if (activeBorrows.Any(r => r.DueDate < DateTime.UtcNow))
            throw new BusinessRuleException("User has overdue books and cannot borrow new ones.");
        if (activeBorrows.Any(r => r.BookId == request.BookId))
            throw new BusinessRuleException("User already has an active borrow for this book.");

        var book = await _db.Books.FindAsync(request.BookId);
        if (book == null)
            throw new NotFoundException($"Book with ID {request.BookId} not found.");

        if (book.AvailableCopies <= 0)
            throw new BusinessRuleException("No available copies of this book.");

        book.AvailableCopies -= 1;

        var record = new BorrowRecord
        {
            UserId = user.Id,
            BookId = book.Id,
            BorrowedAt = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(request.Days)
        };

        _db.BorrowRecords.Add(record);
        await _db.SaveChangesAsync();

        record.User = user;
        record.Book = book;
        return _mapper.Map<BorrowRecordDTO>(record);
    }

    public async Task<BorrowRecordDTO> ReturnBookAsync(int borrowId, int currentUserId, string currentUserRole)
    {
        var record = await _db.BorrowRecords
            .Include(r => r.User)
            .Include(r => r.Book)
            .FirstOrDefaultAsync(r => r.Id == borrowId);

        if (record == null)
            throw new NotFoundException($"Borrow record with ID {borrowId} not found.");

        if (currentUserRole != "Librarian" && record.UserId != currentUserId)
            throw new ForbiddenException("You can only return your own borrowed books.");

        if (record.ReturnedAt != null)
            throw new BusinessRuleException("This book has already been returned.");

        record.ReturnedAt = DateTime.UtcNow;
        record.Book.AvailableCopies += 1;

        await _db.SaveChangesAsync();
        return _mapper.Map<BorrowRecordDTO>(record);
    }

    public async Task<List<BorrowRecordDTO>> GetMyBorrowsAsync(int userId, bool activeOnly)
    {
        var query = _db.BorrowRecords
            .Include(r => r.User)
            .Include(r => r.Book)
            .Where(r => r.UserId == userId);

        if (activeOnly)
            query = query.Where(r => r.ReturnedAt == null);

        var list = await query.OrderByDescending(r => r.BorrowedAt).ToListAsync();
        return _mapper.Map<List<BorrowRecordDTO>>(list);
    }

    public async Task<PagedResult<BorrowRecordDTO>> GetAllBorrowsAsync(bool? overdueOnly, int page, int pageSize)
    {
        var query = _db.BorrowRecords
            .Include(r => r.User)
            .Include(r => r.Book)
            .AsQueryable();

        if (overdueOnly == true)
            query = query.Where(r => r.ReturnedAt == null && r.DueDate < DateTime.UtcNow);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(r => r.BorrowedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<BorrowRecordDTO>
        {
            Items = _mapper.Map<List<BorrowRecordDTO>>(items),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<BorrowRecordDTO> PayFineAsync(int borrowId)
    {
        var record = await _db.BorrowRecords
            .Include(r => r.User)
            .Include(r => r.Book)
            .FirstOrDefaultAsync(r => r.Id == borrowId);

        if (record == null)
            throw new NotFoundException($"Borrow record with ID {borrowId} not found.");
        await _db.SaveChangesAsync();

        return _mapper.Map<BorrowRecordDTO>(record);
    }
}
