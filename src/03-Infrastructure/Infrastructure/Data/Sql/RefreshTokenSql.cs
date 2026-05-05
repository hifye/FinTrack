namespace Infrastructure.Data.Sql;

public class RefreshTokenSql
{
    public const string GetRefreshToken = """
        select id as Id,
        user_id as UserId,
        token as Token,
        expires_at as ExpiresAt,
        is_revoked as IsRevoked,
        created_at as CreatedAt
        from auth.refresh_tokens
        where token = @Token
        """;

    public const string CreateRefreshToken = """
        insert into auth.refresh_tokens (user_id, token, expires_at, is_revoked, created_at)
        values (@UserId, @Token, @ExpiresAt, @IsRevoked, @CreatedAt)
        """;

    public const string RevokeRefreshToken = """
        update auth.refresh_tokens
        set is_revoked = true
        where id = @Id
        """;

    public const string RevokeAllUserTokens = """
        update auth.refresh_tokens
        set is_revoked = true
        where user_id = @UserId
        """;
}
