namespace Application.Interfaces.Services;

public interface ICurrentUserService
{
    public Guid UserId { get; }
}