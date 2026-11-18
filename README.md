README
LIbraryAPI არის ბიბლიოთეკის მენეჯმენტის სისტემა (.NET 8) 
პროექტი მოიცავს მომხმარებელთა ავტორიზაციას (JWT), როლების შესაბამის წვდომის კონსტროლს
(ბიბლიოთეკარი/მკითხველი) , წიგნების, ავტორების, ჟანრების,ბიბლიოთეკის ბარათების და წიგნის გაცემა/დაბრუნების ბიზნეს ლოგიკას.

გამოყენებულია:

ASP.NET Core Web API (.NET 8)
Entity Framework Core (SQLite)
JWT Authentication & Role-Based Authorization
AutoMapper
FluentValidation

გაშვების ინსტრუქცია:
```bash
   git clone <repository-url>
   cd LibraryAPI

JSON:
"ConnectionStrings": {
  "DefaultConnection": "Data Source=library.db"
}

მონაცემთა ბაზის მიგრაცია:
dotnet ef database update

აპლიკაციის გაშვება:
dotnet run

გაშვების შემდეგ Swagger UI ხელმისაწვდომი იქნება ბრაუზერში, სადაც ტოკენის ჩასმა ხდება
ღილაკით Authorize.

 საწყისი მონაცემები:
 როლი-Librarian
 Email-admin@library.com
 Password-Admin123!

 ახლა შეგვიძლია მკითხველის რეგისტრაცია OST/api/auth/register ენდფოინთით.

 ER დიაგრამა;
erDiagram
    User ||--o| LibraryCard : "has (1:1)"
    User ||--o{ BorrowRecord : "borrows"
    Author ||--o{ Book : "writes"
    Book ||--o{ BorrowRecord : "is borrowed in"
    Book }o--o{ Genre : "categorized by (N:M)"

    User {
        int Id PK
        string FullName
        string Email UK
        string PasswordHash
        string Role
        datetime CreatedAt
    }

    LibraryCard {
        int Id PK
        string CardNumber UK
        datetime IssuedAt
        datetime ExpiresAt
        int UserId FK
    }

    Author {
        int Id PK
        string FirstName
        string LastName
        int BirthYear
        string Country
    }

    Genre {
        int Id PK
        string Name UK
    }

    Book {
        int Id PK
        string Title
        string ISBN UK
        int PublishedYear
        int TotalCopies
        int AvailableCopies
        int AuthorId FK
    }

    BorrowRecord {
        int Id PK
        int UserId FK
        int BookId FK
        datetime BorrowedAt
        datetime DueDate
        datetime ReturnedAt
    }
