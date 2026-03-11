using Finansly.Application.DTOs.Transactions;
using Finansly.Application.Interfaces.Categories;
using Finansly.Application.Interfaces.Transactions;
using Finansly.Domain.Entities;
using Finansly.Infrastructure.Services.Transactions;
using FluentAssertions;
using Moq;

namespace Finansly.Tests.Services;

public class TransactionServiceTests
{
    private readonly Mock<ITransactionRepository> _transactionRepoMock;
    private readonly Mock<ICategoryRepository> _categoryRepoMock;
    private readonly TransactionService _service;

    public TransactionServiceTests()
    {
        _transactionRepoMock = new Mock<ITransactionRepository>();
        _categoryRepoMock = new Mock<ICategoryRepository>();
        _service = new TransactionService(_transactionRepoMock.Object, _categoryRepoMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidData_ReturnsTransactionId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var dto = new CreateTransactionDto
        {
            Amount = 100,
            Date = DateTime.UtcNow,
            CategoryId = categoryId,
            Description = "Test"
        };

        _categoryRepoMock
            .Setup(x => x.GetByIdAsync(categoryId))
            .ReturnsAsync(new Category { Id = categoryId, UserId = userId });

        _transactionRepoMock
            .Setup(x => x.AddAsync(It.IsAny<Transaction>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateAsync(userId, dto);

        // Assert
        result.Should().NotBeEmpty();
        _transactionRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_CategoryNotFound_ThrowsException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var dto = new CreateTransactionDto
        {
            Amount = 100,
            CategoryId = categoryId
        };

        _categoryRepoMock
            .Setup(x => x.GetByIdAsync(categoryId))
            .ReturnsAsync((Category?)null);

        // Act
        var act = () => _service.CreateAsync(userId, dto);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_CategoryNotOwnedByUser_ThrowsException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var dto = new CreateTransactionDto
        {
            Amount = 100,
            CategoryId = categoryId
        };

        _categoryRepoMock
            .Setup(x => x.GetByIdAsync(categoryId))
            .ReturnsAsync(new Category { Id = categoryId, UserId = otherUserId });

        // Act
        var act = () => _service.CreateAsync(userId, dto);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}
