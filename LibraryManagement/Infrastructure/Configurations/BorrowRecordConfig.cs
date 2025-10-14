using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Configurations;

public class BorrowRecordConfig : IEntityTypeConfiguration<BorrowRecord>
{
    public void Configure(EntityTypeBuilder<BorrowRecord> builder)
    {
        builder.HasKey(br => br.Id);
        builder.Property(br => br.BorrowedAt).IsRequired();

        builder.HasOne(br => br.Book)
            .WithMany(b => b.BorrowRecords)
            .HasForeignKey(br => br.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(br => br.Member)
            .WithMany(m => m.BorrowRecords)
            .HasForeignKey(br => br.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}