namespace LibraryAPI.Validators;

using FluentValidation;
using LibraryAPI.DTOs.Genres;

public class GenreRequestValidator : AbstractValidator<CreateGenreRequest>
{
    public GenreRequestValidator(){
        RuleFor(x => x.Name).NotEmpty().Length(2, 50); }
}
