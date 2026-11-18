namespace LibraryAPI.Validators;

using FluentValidation;
using LibraryAPI.DTOs.LibraryCards;

public class LibraryCardRequestValidator: AbstractValidator<CreateLibraryCardRequest>
{
    public LibraryCardRequestValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.ValidYears).InclusiveBetween(1, 5);
    }

}
