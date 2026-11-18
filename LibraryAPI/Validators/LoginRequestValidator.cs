namespace LibraryAPI.Validators;

using LibraryAPI.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

public class LoginRequestValidator:AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
