using Application.Abstraction.Queries;
using Application.Features.Catalog.ListItem;
using Domain.Common;
using MediatR;

namespace Application.Features.Catalog.Queries.Category.GetCategoryDetails;

public class GetCategoryDetailsQueryHandler(ICategoryQueries categoryQueries)
    : IRequestHandler<GetCategoryDetailsQuery, Result<CategoryListItem>>
{
    public async Task<Result<CategoryListItem>> Handle(GetCategoryDetailsQuery query, CancellationToken cancellationToken)
    {
        var category = await categoryQueries.GetCategoryDetails(query.Id);
        if (query.Id != category.Id)
            return Result<CategoryListItem>.Failure("Category not found");
        
        return Result<CategoryListItem>.Success(category);
    }
}