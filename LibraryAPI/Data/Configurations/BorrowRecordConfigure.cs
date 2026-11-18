namespace LibraryAPI.Data.Configurations;

using LibraryAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BorrowRecordConfigure:IEntityTypeConfiguration<BorrowRecord>
{
    public void Configure(EntityTypeBuilder<BorrowRecord> builder)
    {
        builder.HasKey(r => r.Id);
        builder.HasOne(r => r.User).WithMany(u => u.BorrowRecords).HasForeignKey(r => r.BookId);
    }
}
