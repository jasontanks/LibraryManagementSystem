using System.Reflection;
using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.EF;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Library> Libraries { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<BorrowRecord> BorrowRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Seed Data
        var centralLibraryId = new Guid("f9a3a3a9-1b1a-4b8a-8f0a-8a8a8a8a8a8a");
        var johnDoeId = new Guid("b9a3a3a9-1b1a-4b8a-8f0a-8a8a8a8a8a8b");
        var janeSmithId = new Guid("c9a3a3a9-1b1a-4b8a-8f0a-8a8a8a8a8a8c");
        var book1Id = new Guid("d9a3a3a9-1b1a-4b8a-8f0a-8a8a8a8a8a8d");

        modelBuilder.Entity<Library>().HasData(new Library
        {
            Id = centralLibraryId,
            Name = "Central City Library",
            BranchLocation = "123 Main St, Central City"
        });

        modelBuilder.Entity<Book>().HasData(
            new Book { Id = book1Id, Title = "The Hitchhiker's Guide to the Galaxy", LibraryId = centralLibraryId },
            new Book { Id = new Guid("e9a3a3a9-1b1a-4b8a-8f0a-8a8a8a8a8a8e"), Title = "Dune", LibraryId = centralLibraryId },
            new Book { Id = new Guid("f9a3a3a9-1b1a-4b8a-8f0a-8a8a8a8a8a8f"), Title = "Foundation", LibraryId = centralLibraryId }
        );

        modelBuilder.Entity<Member>().HasData(
            new Member { Id = johnDoeId, FullName = "John Doe", LibraryId = centralLibraryId },
            new Member { Id = janeSmithId, FullName = "Jane Smith", LibraryId = centralLibraryId }
        );

        modelBuilder.Entity<BorrowRecord>().HasData(
            new BorrowRecord
            {
                Id = Guid.NewGuid(),
                BookId = book1Id,
                MemberId = johnDoeId,
                BorrowedAt = DateTime.UtcNow.AddDays(-10),
                ReturnedAt = null // This book is currently borrowed
            }
        );
    }
}