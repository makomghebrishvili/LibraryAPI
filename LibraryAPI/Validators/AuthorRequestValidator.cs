namespace LibraryAPI.Validators;

using FluentValidation;
using LibraryAPI.DTOs.Authors;

public class AuthorRequestValidator : AbstractValidator<CreateAuthorRequest>
{
    public AuthorRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().Length(1, 60);
        RuleFor(x => x.LastName).NotEmpty().Length(1, 60);
        RuleFor(x => x.BirthYear).InclusiveBetween(1000, DateTime.UtcNow.Year);
        RuleFor(x => x.Country).NotEmpty().Length(2, 60);
    }
}
