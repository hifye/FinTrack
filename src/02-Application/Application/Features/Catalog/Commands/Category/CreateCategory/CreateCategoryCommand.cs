using Domain.Common;
using MediatR;

namespace Application.Features.Catalog.Commands.Category.CreateCategory;

public record CreateCategoryCommand(
    string Name,
    string Type) : IRequest<Result>;