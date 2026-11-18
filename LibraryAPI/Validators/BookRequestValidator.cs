namespace LibraryAPI.Validators;

using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryAPI.DTOs.Books;

public class BookRequestValidator:AbstractValidator<CreateBookRequest>
{
    public BookRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.")
                .Length(1, 200);
        RuleFor(x => x.ISBN).NotEmpty()
            .Matches(@"^\d{10}(\d{3})?$").WithMessage("ISBN must be 10 or 13 digits.");

        RuleFor(x => x.PublishedYear)
            .InclusiveBetween(1450, DateTime.UtcNow.Year);

        RuleFor(x => x.TotalCopies)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.AuthorId)
            .GreaterThan(0);

        RuleFor(x => x.GenreIds) .NotEmpty().WithMessage("At least one genre must be selected.");
    }
}
