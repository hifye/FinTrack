namespace Infrastructure.Data.Sql;

public class RefreshTokenSql
{
    public const string GetRefreshToken = """
                                          select id as Id,
                                          token as Token,
                                          userId as UserId,
                                          expires_at as ExpiresAt,
                                          created_at as CreatedAt,
                                          revoked as IsRevoked
                                          from auth.refresh_tokens
                                          where userId = @UserId
                                          """;
    
    public const string CreateRefreshToken = """
                                            insert into RefreshTokens (id, user_id, token, expires_at, is_revoked, created_at)
                                            values (@Id, @UserId, @Token, @ExpiresAt, @IsRevoked, @CreatedAt)
                                            """;
    
    public const string RevokeRefreshToken = """
                                            update RefreshTokens
                                            set is_revoked = true
                                            where id = @Id
                                            """;

    public const string RevokeAllByUser = """
                                          update RefreshTokens
                                          set is_revoked = true
                                          where userId = @UserId
                                          """;
}