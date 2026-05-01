using Application.Features.Auth.Responses;
using Domain.Common;
using MediatR;

namespace Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<Result<TokenResponse>>;