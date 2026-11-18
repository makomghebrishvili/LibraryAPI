namespace LibraryAPI.Services.Interfaces;

using LibraryAPI.DTOs.Auth;


public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
