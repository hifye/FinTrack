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
                                        insert into finance.accounts (user_id, name, type, initial_balance)
                                        values (@UserId, @Name, @Type, @InitialBalance)
                                        """;

    public const string UpdateAccount = """
                                        update finance.accounts
                                        set name = @Name,
                                            type = @Type,
                                            is_active = @IsActive,
                                        where id = @Id
                                        """;

    public const string DeleteAccount = """
                                        delete from finance.accounts
                                        where id = @Id
                                        """;
}