using System.Data;
using Dapper;
using Domain.ValueObjects;

namespace Infrastructure.Configurations.TypeHandlers;

public class EmailTypeHandler : SqlMapper.TypeHandler<Email>
{
    public override void SetValue(IDbDataParameter parameter, Email? value)
        => parameter.Value = value?.Address;

    public override Email? Parse(object value)
    {
        var result = Email.Create(value.ToString()!);
        if (result.IsFailure)
            throw new DataException($"Invalid email from database: {value}.");
        return result.Value;
    }
}