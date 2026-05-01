using Application.Features.Auth.Responses;
using Domain.Common;
using MediatR;

namespace Application.Features.Auth.Commands.User.Login;

public record LoginCommand(
    string Email,
    string Password) : IRequest<Result<TokenResponse>>;