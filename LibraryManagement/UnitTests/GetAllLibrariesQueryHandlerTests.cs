using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Handlers;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.Application.Libraries.Queries;

public class GetAllLibrariesQueryHandlerTests
{
    private readonly Mock<ILibraryRepository> _libraryRepositoryMock;
    private readonly GetAllLibrariesQueryHandler _handler;

    public GetAllLibrariesQueryHandlerTests()
    {
        _libraryRepositoryMock = new Mock<ILibraryRepository>();
        _handler = new GetAllLibrariesQueryHandler(_libraryRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllLibraries()
    {
        // Arrange
        var query = new GetAllLibrariesQuery();
        var libraries = new List<Library>
        {
            new() { Id = Guid.NewGuid(), Name = "Library 1", BranchLocation = "Downtown" },
            new() { Id = Guid.NewGuid(), Name = "Library 2", BranchLocation = "Uptown" }
        };
        _libraryRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(libraries);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeAssignableTo<IEnumerable<LibraryDto>>();
        result.Count().Should().Be(2);
    }
}