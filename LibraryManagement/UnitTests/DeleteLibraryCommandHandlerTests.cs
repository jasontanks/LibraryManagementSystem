using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.Handlers;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.Application.Libraries.Commands;

public class DeleteLibraryCommandHandlerTests
{
    private readonly Mock<ILibraryRepository> _libraryRepositoryMock;
    private readonly DeleteLibraryCommandHandler _handler;

    public DeleteLibraryCommandHandlerTests()
    {
        _libraryRepositoryMock = new Mock<ILibraryRepository>();
        _handler = new DeleteLibraryCommandHandler(_libraryRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteLibrary_WhenLibraryExists()
    {
        // Arrange
        var command = new DeleteLibraryCommand(Guid.NewGuid());
        var library = new Library { Id = command.Id, Name = "Test Library" };

        _libraryRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync(library);
        _libraryRepositoryMock.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _libraryRepositoryMock.Verify(repo => repo.Delete(library), Times.Once);
        _libraryRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        result.Should().Be(true);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenLibraryDoesNotExist()
    {
        // Arrange
        var command = new DeleteLibraryCommand(Guid.NewGuid());
        _libraryRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync((Library?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(false);
    }
}