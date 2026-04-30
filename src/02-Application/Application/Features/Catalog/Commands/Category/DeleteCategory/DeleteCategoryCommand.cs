using Domain.Common;
using MediatR;

namespace Application.Features.Catalog.Commands.Category.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : IRequest<Result>;