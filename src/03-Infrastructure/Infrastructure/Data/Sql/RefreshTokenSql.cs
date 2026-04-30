namespace Infrastructure.Data.Sql;

public class RefreshTokenSql
{
    public const string GetRefreshToken = """
                                          select id as Id,
                                          token as Token,
                                          userId as UserId,
                                          expires as Expires,
                                          created as Created,
                                          revoked as Revoked
                                          where userId = @UserId
                                          """;
    
    public const string CreateRefreshToken = """
                                            insert into RefreshTokens (token, userId, expires, created)
                                            values (@Token, @UserId, @Expires, @Created)
                                            """;
    
    public const string RevokeRefreshToken = """
                                            update RefreshTokens
                                            set revoked = @Revoked
                                            where userId = @UserId
                                            """;
}