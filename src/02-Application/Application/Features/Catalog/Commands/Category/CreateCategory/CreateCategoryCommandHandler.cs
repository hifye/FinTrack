using Application.Abstraction.Persistance.Repositories.Catalog;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;

namespace Application.Features.Catalog.Commands.Category.CreateCategory;

public class CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser) : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        return await Domain.Entities.Catalog.Category.Create(currentUser.UserId, command.Name, command.Type)
            .BindAsync(async category =>
            {
                await categoryRepository.CreateCategory(category);
                await unitOfWork.CommitAsync();
                return Result<Guid>.Success(category.Id);
            });
    }
}