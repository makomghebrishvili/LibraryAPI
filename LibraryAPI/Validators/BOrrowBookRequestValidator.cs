namespace LibraryAPI.Validators;

using FluentValidation;
using LibraryAPI.DTOs.Borrows;

public class BorrowBookRequestValidator: AbstractValidator<BorrowBookRequest>
{
    public BorrowBookRequestValidator()
    {
        RuleFor(x => x.BookId).GreaterThan(0);
        RuleFor(x => x.Days).InclusiveBetween(1, 60);
    }
}
