using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.Handlers;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using Xunit;

namespace UnitTests.Application.Libraries.Queries;

public class GetLibraryByIdQueryHandlerTests
{
    private readonly Mock<ILibraryRepository> _libraryRepositoryMock;
    private readonly Mock<ILogger<GetLibraryByIdQueryHandler>> _loggerMock;
    private readonly GetLibraryByIdQueryHandler _handler;

    public GetLibraryByIdQueryHandlerTests()
    {
        _libraryRepositoryMock = new Mock<ILibraryRepository>();
        _loggerMock = new Mock<ILogger<GetLibraryByIdQueryHandler>>();
        _handler = new GetLibraryByIdQueryHandler(_libraryRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnLibraryDto_WhenLibraryExists()
    {
        // Arrange
        var query = new GetLibraryByIdQuery(Guid.NewGuid());
        var library = new Library { Id = query.Id, Name = "Test Library", BranchLocation = "Test Branch" };
        _libraryRepositoryMock.Setup(repo => repo.GetByIdAsync(query.Id)).ReturnsAsync(library);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LibraryDto>();
        result.Id.Should().Be(query.Id);
        result.Name.Should().Be(library.Name);
        result.BranchLocation.Should().Be(library.BranchLocation);
    }

    [Fact]
    public async Task Handle_ShouldThrowLibraryNotFoundException_WhenLibraryDoesNotExist()
    {
        // Arrange
        var query = new GetLibraryByIdQuery(Guid.NewGuid());
        _libraryRepositoryMock.Setup(repo => repo.GetByIdAsync(query.Id)).ReturnsAsync((Library?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}