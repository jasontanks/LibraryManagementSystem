using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Configurations;

public class LibraryConfig : IEntityTypeConfiguration<Library>
{
    public void Configure(EntityTypeBuilder<Library> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Name).IsRequired().HasMaxLength(100);
        builder.Property(l => l.BranchLocation).IsRequired().HasMaxLength(200);

        // One-to-Many: Library -> Books
        builder.HasMany(l => l.Books)
            .WithOne(b => b.Library)
            .HasForeignKey(b => b.LibraryId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-Many: Library -> Members
        builder.HasMany(l => l.Members)
            .WithOne(m => m.Library)
            .HasForeignKey(m => m.LibraryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}