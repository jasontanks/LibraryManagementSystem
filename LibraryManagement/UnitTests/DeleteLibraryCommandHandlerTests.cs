using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.Handlers;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using Xunit;

namespace LibraryManagement.Application.UnitTests.Handlers;

public class DeleteLibraryCommandHandlerTests
{
    private readonly Mock<ILibraryRepository> _libraryRepositoryMock;
    private readonly Mock<ILogger<DeleteLibraryCommandHandler>> _loggerMock;
    private readonly DeleteLibraryCommandHandler _handler;

    public DeleteLibraryCommandHandlerTests()
    {
        _libraryRepositoryMock = new Mock<ILibraryRepository>();
        _loggerMock = new Mock<ILogger<DeleteLibraryCommandHandler>>();
        _handler = new DeleteLibraryCommandHandler(_libraryRepositoryMock.Object, _loggerMock.Object);
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
        _libraryRepositoryMock.Verify(repo => repo.Delete(It.Is<Library>(l => l.Id == command.Id)), Times.Once);
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