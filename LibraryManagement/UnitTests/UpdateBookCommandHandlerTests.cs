using FluentAssertions;
using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Handlers;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using Moq;
using Microsoft.Extensions.Logging;
using Xunit;

namespace LibraryManagement.Application.UnitTests.Handlers;

public class UpdateBookCommandHandlerTests
{
    private readonly Mock<IBookRepository> _mockBookRepository;
    private readonly Mock<ILogger<UpdateBookCommandHandler>> _loggerMock;
    private readonly UpdateBookCommandHandler _handler;

    public UpdateBookCommandHandlerTests()
    {
        _mockBookRepository = new Mock<IBookRepository>();
        _loggerMock = new Mock<ILogger<UpdateBookCommandHandler>>();
        _handler = new UpdateBookCommandHandler(_mockBookRepository.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFalse_WhenBookNotFound()
    {
        // Arrange
        var command = new UpdateBookCommand { Id = Guid.NewGuid(), Title = "Updated Title" };
        _mockBookRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((Book?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_Should_UpdateBookAndSaveChanges_WhenBookExists()
    {
        // Arrange
        var book = new Book { Id = Guid.NewGuid(), Title = "Original Title" };
        var command = new UpdateBookCommand { Id = book.Id, Title = "Updated Title" };
        _mockBookRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(book);
        _mockBookRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        book.Title.Should().Be("Updated Title");
        _mockBookRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}