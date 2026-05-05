using Application.Abstraction.Persistance.Repositories.Catalog;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;

namespace Application.Features.Catalog.Commands.Category.PatchCategory;

public class PatchCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
    : IRequestHandler<PatchCategoryCommand, Result>
{
    public async Task<Result> Handle(PatchCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetCategoryById(command.Id);
        if(category is null)
            return Result.Failure("Category not found.");
        
        var result = category.Patch(command.Name, command.Type, command.IsActive);
        if (result.IsFailure)
            return result;
        
        await categoryRepository.UpdateCategory(category);
        await unitOfWork.CommitAsync();
        return Result.Success();
    }
}