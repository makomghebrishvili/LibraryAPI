namespace LibraryAPI.DTOs.Auth;

public class AuthResponse
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string AccessToken {  get; set; } = string.Empty;
    public DateTime ExpireAt { get; set; }
}
