namespace LibraryAPI.Data.Seed;

using LibraryAPI.Entities;
using Microsoft.EntityFrameworkCore;

public class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();
        if(!await db.Users.AnyAsync())
        {
            db.Users.Add(new User
            {
                Email = "admin@library.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                FullName = "Library Admin",
                Role = "Librarian",
                CreatedAt = DateTime.UtcNow

            });

        }

        if(!await db.Genres.AnyAsync())
        {
            db.Genres.AddRange(
                new Genre { Name = "fantasy" },
                new Genre { Name = "historical" },
                new Genre { Name = "detective" },
                new Genre { Name = "classic" },
                new Genre { Name = "science" }
                );
        }
        await db.SaveChangesAsync();
    }
}
