namespace LibraryAPI.Data.Configurations;

using LibraryAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BookConfiguration: IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(k => k.Id);
        builder.Property(k => k.Title).HasMaxLength(200).IsRequired();
        builder.Property(k => k.ISBN).HasMaxLength(13).IsRequired();
        builder.HasIndex(k => k.ISBN).IsUnique();
        builder.HasOne(k => k.Author).WithMany(a => a.Books).HasForeignKey(k => k.AuthorId).OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(k => k.Genres).WithMany(g => g.Books).UsingEntity(j => j.ToTable("Book Genres"));

    }
}
