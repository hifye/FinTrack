using Domain.Common;
using MediatR;

namespace Application.Features.Auth.Commands.User.Logout;

public record LogoutCommand(Guid UserId) : IRequest<Result>;
