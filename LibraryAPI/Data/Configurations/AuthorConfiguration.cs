namespace LibraryAPI.Data.Configurations;

using LibraryAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AuthorConfiguration: IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.FirstName).HasMaxLength(60).IsRequired();
        builder.Property(a => a.LastName).HasMaxLength(60).IsRequired();
        builder.Property(a => a.Country).HasMaxLength(60).IsRequired();
    }
}
