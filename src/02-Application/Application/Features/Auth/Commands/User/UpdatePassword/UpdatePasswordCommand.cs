using Domain.Common;
using MediatR;

namespace Application.Features.Auth.Commands.User.UpdatePassword;

public record UpdatePasswordCommand(string Password, string ConfirmPassword) : IRequest<Result>;