using System.Data;
using Dapper;
using Domain.ValueObjects;

namespace Infrastructure.Configurations.TypeHandlers;

/// <summary>
/// Manipulador de tipo para mapear o objeto de valor <see cref="Price"/> em consultas do Dapper.
/// </summary>
public class PriceTypeHandler : SqlMapper.TypeHandler<Price>
{
    /// <summary>
    /// Define o valor do parâmetro do banco de dados a partir de uma instância de <see cref="Price"/>.
    /// </summary>
    /// <param name="parameter">O parâmetro do banco de dados.</param>
    /// <param name="value">A instância de preço.</param>
    public override void SetValue(IDbDataParameter parameter, Price? value)
        => parameter.Value = value?.Value;

    /// <summary>
    /// Converte o valor vindo do banco de dados em uma instância de <see cref="Price"/>.
    /// </summary>
    /// <param name="value">O valor bruto do banco de dados.</param>
    /// <returns>Uma instância validada de <see cref="Price"/>.</returns>
    /// <exception cref="DataException">Lançada se o valor do banco de dados não for um preço válido.</exception>
    public override Price? Parse(object value)
    {
        var result = Price.Create(Convert.ToDecimal(value));
        if (result.IsFailure)
            throw new DataException($"Invalid price from database: {value}.");
        return result.Value;
    }
}