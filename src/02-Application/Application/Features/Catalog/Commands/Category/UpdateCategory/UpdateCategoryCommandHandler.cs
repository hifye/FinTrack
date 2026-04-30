using Application.Abstraction.Persistance.Repositories.Catalog;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;

namespace Application.Features.Catalog.Commands.Category.UpdateCategory;

public class UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateCategoryCommand, Result>
{
    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetCategoryById(command.Id);

        return await (category is null ? Result.Failure("Category not found.") : Result.Success())
            .Bind(() => category!.Update(command.Id, command.Name, command.Type, command.IsActive))
            .BindAsync(async () =>
            {
                await categoryRepository.UpdateCategory(category!);
                await unitOfWork.CommitAsync();
                return Result.Success();
            });
    }
}