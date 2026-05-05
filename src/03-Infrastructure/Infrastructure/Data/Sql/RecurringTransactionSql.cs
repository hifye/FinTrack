namespace Infrastructure.Data.Sql;

public class RecurringTransactionSql
{
    public const string GetRecurringTransactionById = """
        select id as Id,
           user_id as UserId,
           account_id as AccountId,
           category_id as CategoryId,
           amount as Amount,
           frequency as Frequency,
           next_occurrence as NextOccurrence,
           is_active as IsActive,
           created_at as CreatedAt
        from finance.recurring_transactions
        where id = @Id
        """;

    public const string CreateRecurringTransaction = """
        insert into finance.recurring_transactions (user_id, account_id, category_id, amount, type, description, frequency, next_occurrence, start_date, end_date, is_active, created_at)
        values (@UserId, @AccountId, @CategoryId, @Amount, @Type, @Description, @Frequency, @NextOccurrence, @StartDate, @EndDate, @IsActive, @CreatedAt)
        """;

    public const string UpdateRecurringTransaction = """
        update finance.recurring_transactions
        set amount = @Amount,
            type = @Type,
            description = @Description,
            frequency = @Frequency,
            is_active = @IsActive
        where id = @Id
        """;

    public const string DeleteRecurringTransaction = """
        update finance.recurring_transactions
        set is_active = false
        where id = @Id
        """;
}
