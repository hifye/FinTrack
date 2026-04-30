using Domain.Common;

namespace Domain.Entities.Auth;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    private RefreshToken(Guid userId, string token)
    {
        UserId = userId;
        Token = token;
    }

    public static Result<RefreshToken> Create(Guid userId, string token)
    {
        return Guard.AgainstOutOfRange(userId == Guid.Empty, "The User id cannot be empty")
            .Bind(() => Guard.AgainstNullOrWhiteSpace(token, "Token cannot be empty"))
            .Map(() => new RefreshToken(userId, token));
    }
    
    protected RefreshToken() { }
}