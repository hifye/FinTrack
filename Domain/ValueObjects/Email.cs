using System.Net.Mail;
using Domain.Commom;

namespace Domain.ValueObjects;

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