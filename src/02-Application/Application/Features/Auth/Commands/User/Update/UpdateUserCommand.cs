using Domain.Common;
using MediatR;

namespace Application.Features.Auth.Commands.User.Update;

public record UpdateUserCommand(string Password, string ConfirmPassword) : IRequest<Result>;