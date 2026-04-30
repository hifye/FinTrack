using Domain.Common;
using MediatR;

namespace Application.Features.Catalog.Commands.Category.UpdateCategory;

public record UpdateCategoryCommand(
    Guid Id,
    string Name,
    string Type,
    bool IsActive) : IRequest<Result>;