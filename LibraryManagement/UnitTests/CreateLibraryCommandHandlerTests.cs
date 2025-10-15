using LibraryManagement.Application.Commands;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Handlers;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.Application.Libraries.Commands;

public class CreateLibraryCommandHandlerTests
{
    private readonly Mock<ILibraryRepository> _libraryRepositoryMock;
    private readonly CreateLibraryCommandHandler _handler;

    public CreateLibraryCommandHandlerTests()
    {
        _libraryRepositoryMock = new Mock<ILibraryRepository>();
        _handler = new CreateLibraryCommandHandler(_libraryRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateLibraryAndReturnLibraryDto()
    {
        // Arrange
        var command = new CreateLibraryCommand { Name = "Central Library", BranchLocation = "Downtown" };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _libraryRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Library>(l => l.Name == command.Name && l.BranchLocation == command.BranchLocation)), Times.Once);
        _libraryRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        result.Should().BeOfType<LibraryDto>();
        result.Name.Should().Be(command.Name);
        result.Id.Should().NotBe(Guid.Empty);
    }
}