namespace LibraryAPI.Controllers;
using LibraryAPI.DTOs.Borrows;
using LibraryAPI.DTOs.Common;
using LibraryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


[ApiController]
[Route("api/borrows")]
[Authorize]
public class BorrowsController : ControllerBase
{
    private readonly IBorrowService _borrowService;

    public BorrowsController(IBorrowService borrowService)
    {
        _borrowService = borrowService;
    }

    private int GetCurrentUserId() =>
        int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

    private string GetCurrentUserRole() =>
        User.FindFirst(ClaimTypes.Role)!.Value;

    [HttpPost]
    public async Task<ActionResult<BorrowRecordDTO>> Borrow(BorrowBookRequest request)
    {
        var record = await _borrowService.BorrowBookAsync(GetCurrentUserId(), GetCurrentUserRole(), request);
        return Created("", record);
    }

    [HttpPost("{id}/return")]
    public async Task<ActionResult<BorrowRecordDTO>> Return(int id)
    {
        return Ok(await _borrowService.ReturnBookAsync(id, GetCurrentUserId(), GetCurrentUserRole()));
    }

    [HttpGet("my")]
    public async Task<ActionResult<List<BorrowRecordDTO>>> GetMyBorrows([FromQuery] bool activeOnly = false)
    {
        return Ok(await _borrowService.GetMyBorrowsAsync(GetCurrentUserId(), activeOnly));
    }

    [HttpGet]
    [Authorize(Roles = "Librarian")]
    public async Task<ActionResult<PagedResult<BorrowRecordDTO>>> GetAll(
        [FromQuery] bool? overdueOnly,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        return Ok(await _borrowService.GetAllBorrowsAsync(overdueOnly, page, pageSize));
    }
}
