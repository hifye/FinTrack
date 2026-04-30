using Domain.Common;

namespace Domain.ValueObjects;

/// <summary>
/// Representa um valor monetário como um objeto de valor dentro do modelo de domínio.
/// A classe <see cref="Price"/> encapsula o comportamento para criar e validar valores de preço,
/// garantindo que eles atendam a certas regras de negócio, como não serem negativos.
/// </summary>
public record Price
{
    public decimal Value { get; }
    
    private Price(decimal value) => Value = value;

    public static Result<Price> Create(decimal value)
    {
        return Guard
            .AgainstOutOfRange(value < 1, "Price cannot be negative.")
            .Map(() => new Price(value));
    }
}