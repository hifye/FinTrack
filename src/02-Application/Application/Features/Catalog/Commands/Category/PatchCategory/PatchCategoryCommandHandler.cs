using Application.Abstraction.Persistance.Repositories.Catalog;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Catalog.Commands.Category.PatchCategory;

public class PatchCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, ILogger<PatchCategoryCommandHandler> logger)
    : IRequestHandler<PatchCategoryCommand, Result>
{
    public async Task<Result> Handle(PatchCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetCategoryById(command.Id);
        if(category is null)
        {
            logger.LogWarning("Category not found for id {CategoryId}", command.Id);
            return Result.Failure("Category not found.", ErrorType.NotFound);
        }
        
        var result = category.Patch(command.Name, command.Type, command.IsActive);
        if (result.IsFailure)
        {
            logger.LogWarning("Failed to update category {CategoryId}", command.Id);
            return result;
        }
        
        await categoryRepository.UpdateCategory(category);
        await unitOfWork.CommitAsync();
        return Result.Success();
    }
}