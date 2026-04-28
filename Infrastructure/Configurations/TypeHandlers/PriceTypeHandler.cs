using System.Data;
using Dapper;
using Domain.ValueObjects;

namespace Infrastructure.Configurations.TypeHandlers;

public class PriceTypeHandler : SqlMapper.TypeHandler<Price>
{
    public override void SetValue(IDbDataParameter parameter, Price? value)
        => parameter.Value = value?.Value;

    public override Price? Parse(object value)
    {
        var result = Price.Create(Convert.ToDecimal(value));
        if (result.IsFailure)
            throw new DataException($"Invalid price from database: {value}.");
        return result.Value;
    }
}