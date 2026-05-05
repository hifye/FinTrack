using Application.Features.Catalog.ListItem;
using Domain.Common;
using MediatR;

namespace Application.Features.Catalog.Queries.Category.GetCategoryDetails;

public record GetCategoryDetailsQuery(Guid Id) : IRequest<Result<CategoryListItem>>;
