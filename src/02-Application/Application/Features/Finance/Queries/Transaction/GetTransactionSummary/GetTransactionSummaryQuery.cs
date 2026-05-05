using Application.Features.Finance.ListItem;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Queries.Transaction.GetTransactionSummary;

public record GetTransactionSummaryQuery(DateTime StartDate, DateTime EndDate) : IRequest<Result<TransactionSummary>>;