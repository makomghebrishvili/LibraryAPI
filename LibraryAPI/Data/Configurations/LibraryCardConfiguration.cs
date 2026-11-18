namespace LibraryAPI.Data.Configurations;

using LibraryAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class LibraryCardConfiguration: IEntityTypeConfiguration<LibraryCard>
{
    public void Configure (EntityTypeBuilder<LibraryCard> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.CardNumber).HasMaxLength(20).IsRequired();
        builder.HasIndex(l => l.CardNumber).IsUnique();
        builder.HasOne(l => l.User).WithOne(u => u.LibraryCard).HasForeignKey<LibraryCard>(l => l.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(l=>l.UserId).IsUnique();
    }
}

