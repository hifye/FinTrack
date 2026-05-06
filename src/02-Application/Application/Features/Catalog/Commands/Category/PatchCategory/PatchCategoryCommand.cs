using System.Text.Json.Serialization;
using Domain.Common;
using MediatR;

namespace Application.Features.Catalog.Commands.Category.PatchCategory;

public record PatchCategoryCommand([property: JsonIgnore] Guid Id, string? Name, string? Type, bool? IsActive)
    : IRequest<Result>;
