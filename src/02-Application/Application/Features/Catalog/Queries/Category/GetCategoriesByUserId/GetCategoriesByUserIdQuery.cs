using Application.Features.Catalog.ListItem;
using Domain.Common;
using MediatR;

namespace Application.Features.Catalog.Queries.Category.GetCategoriesByUserId;

public record GetCategoriesByUserIdQuery : IRequest<Result<IReadOnlyList<CategoryListItem>>>;