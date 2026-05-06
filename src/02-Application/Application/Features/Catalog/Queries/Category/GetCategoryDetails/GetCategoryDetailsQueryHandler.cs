using Application.Abstraction.Queries;
using Application.Features.Catalog.ListItem;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Catalog.Queries.Category.GetCategoryDetails;

public class GetCategoryDetailsQueryHandler(ICategoryQueries categoryQueries, ILogger<GetCategoryDetailsQueryHandler> logger)
    : IRequestHandler<GetCategoryDetailsQuery, Result<CategoryListItem>>
{
    public async Task<Result<CategoryListItem>> Handle(GetCategoryDetailsQuery query, CancellationToken cancellationToken)
    {
        var category = await categoryQueries.GetCategoryDetails(query.Id);

        if (category is not null) return Result<CategoryListItem>.Success(category);
        logger.LogWarning("Category with ID {CategoryId} not found", query.Id);
        return Result<CategoryListItem>.Failure("Category not found", ErrorType.NotFound);

    }
}