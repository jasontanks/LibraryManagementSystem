using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Configurations;

public class MemberConfig : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.FullName).IsRequired().HasMaxLength(100);

        // One-to-Many: Member -> BorrowRecords
        builder.HasMany(m => m.BorrowRecords)
            .WithOne(br => br.Member)
            .HasForeignKey(br => br.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}