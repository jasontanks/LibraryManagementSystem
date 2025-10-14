using FluentAssertions;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.Handlers;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using Moq;
using Xunit;

namespace LibraryManagement.Application.UnitTests.Handlers;

public class GetMemberByIdQueryHandlerTests
{
    private readonly Mock<IMemberRepository> _mockMemberRepository;
    private readonly GetMemberByIdQueryHandler _handler;

    public GetMemberByIdQueryHandlerTests()
    {
        _mockMemberRepository = new Mock<IMemberRepository>();
        _handler = new GetMemberByIdQueryHandler(_mockMemberRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_ThrowNotFoundException_WhenMemberDoesNotExist()
    {
        // Arrange
        var query = new GetMemberByIdQuery(Guid.NewGuid());
        _mockMemberRepository.Setup(r => r.GetByIdAsync(query.Id)).ReturnsAsync((Member?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_ReturnMemberDto_WhenMemberExists()
    {
        // Arrange
        var member = new Member { Id = Guid.NewGuid(), FullName = "Test Member" };
        var query = new GetMemberByIdQuery(member.Id);
        _mockMemberRepository.Setup(r => r.GetByIdAsync(query.Id)).ReturnsAsync(member);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(member.Id);
        result.FullName.Should().Be(member.FullName);
    }
}