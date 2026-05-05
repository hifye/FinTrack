namespace Infrastructure.Data.Sql;

public class AccountSql
{
    public const string GetAccountById = """
        select id as Id,
           user_id as UserId,
           name as Name,
           type as Type,
           initial_balance as InitialBalance,
           current_balance as CurrentBalance,
           is_active as IsActive,
           created_at as CreatedAt
         from finance.accounts
         where id = @Id
        """;

    public const string CreateAccount = """
        insert into finance.accounts (user_id, name, type, initial_balance, current_balance, is_active, created_at)
        values (@UserId, @Name, @Type, @InitialBalance, @CurrentBalance, @IsActive, @CreatedAt)
        """;

    public const string UpdateAccount = """
        update finance.accounts
        set type = @Type,
            is_active = @IsActive
        where id = @Id
        """;

    public const string DeleteAccount = """
        update finance.accounts
        set is_active = false
        where id = @Id
        """;
}
