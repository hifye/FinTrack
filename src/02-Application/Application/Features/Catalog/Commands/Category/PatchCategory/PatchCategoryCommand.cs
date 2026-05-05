using Domain.Common;
using MediatR;

namespace Application.Features.Catalog.Commands.Category.PatchCategory;

public record PatchCategoryCommand(Guid Id, string? Name, string? Type, bool? IsActive) : IRequest<Result>;