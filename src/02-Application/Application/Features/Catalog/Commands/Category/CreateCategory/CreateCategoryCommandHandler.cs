using Application.Abstraction.Persistance.Repositories.Catalog;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;

namespace Application.Features.Catalog.Commands.Category.CreateCategory;

public class CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCategoryCommand, Result>
{
    public async Task<Result> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        return await Domain.Entities.Catalog.Category.Create(command.UserId, command.Name, command.Type)
            .BindAsync(async category =>
            {
                await categoryRepository.CreateCategory(category);
                await unitOfWork.CommitAsync();
                return Result.Success();
            });
    }
}