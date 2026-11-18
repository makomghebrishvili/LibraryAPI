namespace LibraryAPI.Validators;

using FluentValidation;
using LibraryAPI.DTOs.Auth;

public class RegisterRequestValidator:AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Password is required.").MinimumLength(8)
            .WithMessage("password must be at least 8 characters")
            .Matches("[a-z]").WithMessage("password must contain at least one uppercase letter")
            .Matches("[0-9]").WithMessage("password must contain at least one digit");

        RuleFor(x => x.FullName).NotEmpty().WithMessage("name is required")
            .Length(2, 100).WithMessage("Full name must be between 2 and 100 characters");

        RuleFor(x => x.Role).Must(u => u == "Reader" || u == "Librarian").WithMessage("Role must be either -Reader- or -Librarian-");
    }
}
