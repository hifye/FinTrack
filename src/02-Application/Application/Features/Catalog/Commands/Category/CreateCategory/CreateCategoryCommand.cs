using Domain.Common;
using MediatR;

namespace Application.Features.Catalog.Commands.Category.CreateCategory;

public record CreateCategoryCommand(
    Guid UserId,
    string Name,
    string Type) : IRequest<Result>;