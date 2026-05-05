using Application.Abstraction.Queries;
using Application.Features.Catalog.ListItem;
using Application.Interfaces.Services;
using Domain.Common;
using MediatR;

namespace Application.Features.Catalog.Queries.Category.GetCategoriesByUserId;

public class GetCategoriesByUserIdQueryHandler(ICategoryQueries categoryQueries, ICurrentUserService currentUser)
    : IRequestHandler<GetCategoriesByUserIdQuery, Result<IReadOnlyList<CategoryListItem>>>
{
    public async Task<Result<IReadOnlyList<CategoryListItem>>> Handle(GetCategoriesByUserIdQuery query, CancellationToken cancellationToken)
    {
        var categories = await categoryQueries.GetCategoriesByUserId(currentUser.UserId);
        if(!categories.Any())
            return Result<IReadOnlyList<CategoryListItem>>.Failure("No categories found for the user");
        
        return Result<IReadOnlyList<CategoryListItem>>.Success(categories);
    }
}