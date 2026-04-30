using Application.Abstraction.Persistance.Repositories.Catalog;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;

namespace Application.Features.Catalog.Commands.Category.DeleteCategory;

public class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var deleted = await categoryRepository.DeleteCategory(request.Id);
        if (!deleted)
            return Result.Failure("Category not found.");
        
        await unitOfWork.CommitAsync();
        return Result.Success();
    }
}