using System.Data;
using Dapper;
using Domain.ValueObjects;

namespace Infrastructure.Configurations.TypeHandlers;

/// <summary>
/// Manipulador de tipo para mapear o objeto de valor <see cref="Email"/> em consultas do Dapper.
/// </summary>
public class EmailTypeHandler : SqlMapper.TypeHandler<Email>
{
    /// <summary>
    /// Define o valor do parâmetro do banco de dados a partir de uma instância de <see cref="Email"/>.
    /// </summary>
    /// <param name="parameter">O parâmetro do banco de dados.</param>
    /// <param name="value">A instância de e-mail.</param>
    public override void SetValue(IDbDataParameter parameter, Email? value)
        => parameter.Value = value?.Address;

    /// <summary>
    /// Converte o valor vindo do banco de dados em uma instância de <see cref="Email"/>.
    /// </summary>
    /// <param name="value">O valor bruto do banco de dados.</param>
    /// <returns>Uma instância validada de <see cref="Email"/>.</returns>
    /// <exception cref="DataException">Lançada se o valor do banco de dados não for um e-mail válido.</exception>
    public override Email? Parse(object value)
    {
        var result = Email.Create(value.ToString()!);
        if (result.IsFailure)
            throw new DataException($"Invalid email from database: {value}.");
        return result.Value;
    }
}