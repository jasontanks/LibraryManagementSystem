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

public class UpdateLibraryCommandHandlerTests
{
    private readonly Mock<ILibraryRepository> _libraryRepositoryMock;
    private readonly Mock<ILogger<UpdateLibraryCommandHandler>> _loggerMock;
    private readonly UpdateLibraryCommandHandler _handler;

    public UpdateLibraryCommandHandlerTests()
    {
        _libraryRepositoryMock = new Mock<ILibraryRepository>();
        _loggerMock = new Mock<ILogger<UpdateLibraryCommandHandler>>();
        _handler = new UpdateLibraryCommandHandler(_libraryRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateLibrary_WhenLibraryExists()
    {
        // Arrange
        var command = new UpdateLibraryCommand { Id = Guid.NewGuid(), Name = "New Name", BranchLocation = "New Location" };
        var library = new Library { Id = command.Id, Name = "Old Name", BranchLocation = "Old Location" };

        _libraryRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync(library);
        _libraryRepositoryMock.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        library.Name.Should().Be(command.Name);
        library.BranchLocation.Should().Be(command.BranchLocation);
        _libraryRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        result.Should().Be(true);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenLibraryDoesNotExist()
    {
        // Arrange
        var command = new UpdateLibraryCommand { Id = Guid.NewGuid(), Name = "New Name" };
        _libraryRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync((Library?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(false);
    }
}