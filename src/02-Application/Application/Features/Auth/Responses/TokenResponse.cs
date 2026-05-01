namespace Application.Features.Auth.Responses;

public record TokenResponse( 
    string Token,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt);