namespace Infrastructure.Data.Sql;

public static class TransactionSql
{
    public const string GetTransactionById = """
                                             select id as Id,
                                             user_id as UserId,
                                             account_id as AccountId,
                                             category_id as CategoryId,
                                             recurring_id as RecurringId,
                                             amount as Amount,
                                             type as Type,
                                             description as Description
                                             where id = @Id
                                             """;
    
    public const string CreateTransaction = """
                                            insert into finance.transactions (user_id, account_id, category_id, recurring_id, amount, type, description)
                                            values (@UserId, @AccountId, @CategoryId, @RecurringId, @Amount, @Type, @Description)
                                            """;

    public const string UpdateTransaction = """
                                            update finance.transactions
                                            set amount = @Amount,
                                                type = @Type,
                                                description = @Description
                                            where id = @Id
                                            """;

    public const string DeleteTransaction = """
                                            delete from finance.transactions
                                            where id = @Id
                                            """;
}