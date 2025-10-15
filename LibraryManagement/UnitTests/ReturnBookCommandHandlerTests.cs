using FluentAssertions;
using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Handlers;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using Moq;
using Microsoft.Extensions.Logging;
using Xunit;

namespace LibraryManagement.Application.UnitTests.Handlers;

public class ReturnBookCommandHandlerTests
{
    private readonly Mock<IBorrowRecordRepository> _mockBorrowRecordRepository;
    private readonly Mock<ILogger<ReturnBookCommandHandler>> _loggerMock;
    private readonly ReturnBookCommandHandler _handler;

    public ReturnBookCommandHandlerTests()
    {
        _mockBorrowRecordRepository = new Mock<IBorrowRecordRepository>();
        _loggerMock = new Mock<ILogger<ReturnBookCommandHandler>>();
        _handler = new ReturnBookCommandHandler(_mockBorrowRecordRepository.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFalse_WhenBorrowRecordNotFound()
    {
        // Arrange
        var command = new ReturnBookCommand { Id = Guid.NewGuid() };
        _mockBorrowRecordRepository
            .Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync((BorrowRecord?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_Should_SetReturnedAtAndSaveChanges_WhenRecordIsValid()
    {
        // Arrange
        var borrowRecord = new BorrowRecord { Id = Guid.NewGuid(), ReturnedAt = null };
        var command = new ReturnBookCommand { Id = borrowRecord.Id };
        _mockBorrowRecordRepository
            .Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(borrowRecord);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        borrowRecord.ReturnedAt.Should().NotBeNull();
        _mockBorrowRecordRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}