using Domain.Common;
using MediatR;

namespace Application.Features.Auth.Commands.User.Register;

public record RegisterCommand(
    string Name,
    string Email,
    string Password,
    string ConfirmPassword) : IRequest<Result>;