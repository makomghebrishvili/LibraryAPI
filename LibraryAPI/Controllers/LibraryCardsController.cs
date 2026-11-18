namespace LibraryAPI.Controllers;

using LibraryAPI.DTOs.LibraryCards;
using LibraryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

[ApiController]
[Route("api/cards")]
[Authorize]
public class LibraryCardsController:ControllerBase
{
    private readonly ILibraryCardService _cardService;

    public LibraryCardsController(ILibraryCardService cardService)
    {
        _cardService = cardService;
    }

    private int GetCurrentUserId() =>
        int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

    [HttpPost]
    [Authorize(Roles = "Librarian")]
    public async Task<ActionResult<LibraryCardDTO>> IssueCard(CreateLibraryCardRequest request)
    {
        var card = await _cardService.IssueCardAsync(request);
        return Created("", card);
    }

    [HttpGet("my")]
    public async Task<ActionResult<LibraryCardDTO>> GetMyCard()
    {
        return Ok(await _cardService.GetMyCardAsync(GetCurrentUserId()));
    }

    [HttpGet("user/{userId}")]
    [Authorize(Roles = "Librarian")]
    public async Task<ActionResult<LibraryCardDTO>> GetByUserId(int userId)
    {
        return Ok(await _cardService.GetByUserIdAsync(userId));
    }

    [HttpPost("user/{userId}/renew")]
    [Authorize(Roles = "Librarian")]
    public async Task<ActionResult<LibraryCardDTO>> RenewCard(int userId, [FromQuery] int years = 1)
    {
        return Ok(await _cardService.RenewCardAsync(userId, years));
    }
}

