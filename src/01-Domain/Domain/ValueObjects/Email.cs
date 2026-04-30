using System.Net.Mail;
using Domain.Common;

namespace Domain.ValueObjects;

/// <summary>
/// Representa um objeto de valor de e-mail imutável.
/// </summary>
/// <remarks>
/// Esta classe encapsula o conceito de um endereço de e-mail válido,
/// garantindo que qualquer instância da classe Email seja validada durante a criação.
/// Os endereços de e-mail são normalizados durante a instanciação (removendo espaços em branco e convertendo para minúsculas).
/// </remarks>
/// <example>
/// Fornece uma fábrica estática para validar e construir uma instância de Email.
/// Exemplos de verificações de validação incluem:
/// - Garantir que o e-mail não seja nulo ou apenas espaços em branco.
/// - Garantir que o e-mail não exceda 100 caracteres de comprimento.
/// - Garantir que o e-mail adira ao formato padrão de e-mail.
/// </example>
public record Email
{
    public string Address { get; } = null!;

    private Email(string address) => Address = address;

    public static Result<Email> Create(string address)
    {
        address = address.Trim().ToLower();

        return Guard
            .AgainstNullOrWhiteSpace(address, "Email cannot be empty")
            .Bind(() =>
                Guard.AgainstOutOfRange(address.Length > 100,
                    "Email cannot be longer than 100 characters."
                )
            )
            .Bind(() => Result.Try(() => new MailAddress(address), "Invalid Email"))
            .Map(() => new Email(address));
    }
}