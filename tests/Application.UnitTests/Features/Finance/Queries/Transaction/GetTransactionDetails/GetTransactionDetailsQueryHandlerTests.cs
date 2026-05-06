using Application.Abstraction.Queries;
using Application.Features.Finance.ListItem;
using Application.Features.Finance.Queries.Transaction.GetTransactionDetails;
using Domain.Common;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Features.Finance.Queries.Transaction.GetTransactionDetails;

/// <summary>
/// Contém testes unitários para o <see cref="GetTransactionDetailsDetailsQueryHandler"/>.
/// </summary>
public class GetTransactionDetailsQueryHandlerTests
{
    private readonly Mock<ITransactionQueries> _transactionQueriesMock;
    private readonly Mock<ILogger<GetTransactionDetailsDetailsQueryHandler>> _loggerMock;
    private readonly GetTransactionDetailsDetailsQueryHandler _handler;

    public GetTransactionDetailsQueryHandlerTests()
    {
        _transactionQueriesMock = new Mock<ITransactionQueries>();
        _loggerMock = new Mock<ILogger<GetTransactionDetailsDetailsQueryHandler>>();
        _handler = new GetTransactionDetailsDetailsQueryHandler(
            _transactionQueriesMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenTransactionExists()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var query = new GetTransactionDetailsQuery(transactionId);
        var transactionItem = new TransactionListItem(
            transactionId, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 
            100m, "Gasto", "Lanche", DateTime.UtcNow, DateTime.UtcNow);
        
        _transactionQueriesMock.Setup(q => q.GetTransactionDetails(transactionId))
            .ReturnsAsync(transactionItem);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(transactionItem);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenTransactionNotFound()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var query = new GetTransactionDetailsQuery(transactionId);
        var differentId = Guid.NewGuid();
        var transactionItem = new TransactionListItem(
            differentId, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 
            100m, "Gasto", "Lanche", DateTime.UtcNow, DateTime.UtcNow);
        
        _transactionQueriesMock.Setup(q => q.GetTransactionDetails(transactionId))
            .ReturnsAsync(transactionItem);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Transaction not found");
        result.ErrorType.Should().Be(ErrorType.NotFound);
    }
}
