using Application.Abstraction.Persistance.Repositories.Catalog;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Catalog.Commands.Category.DeleteCategory;

public class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, ILogger<DeleteCategoryCommandHandler> logger)
    : IRequestHandler<DeleteCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
            var deleted = await categoryRepository.DeleteCategory(command.Id);
            if (!deleted)
            {
                logger.LogWarning("Category not found for id {CategoryId}", command.Id);
                return Result.Failure("Category not found.", ErrorType.NotFound);
            }
            
            await unitOfWork.CommitAsync();
            return Result.Success();
    }
}