using System.Security.Cryptography;
using System.Text;
using Application.Interfaces.Services;
using Konscious.Security.Cryptography;

namespace Infrastructure.Security;

/// <summary>
/// Provedor de hashing de senhas que utiliza o algoritmo Argon2id para garantir alta segurança.
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 32;
    private const int HashSize = 32;
    private const int Iterations = 4;
    private const int Memory = 65536;
    private const int Parallelism = 8;

    /// <summary>
    /// Gera um hash seguro para a senha fornecida usando Argon2id.
    /// </summary>
    /// <param name="password">A senha em texto plano a ser hasheada.</param>
    /// <returns>Uma string formatada contendo o algoritmo, parâmetros, salt e o hash resultante.</returns>
    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = Parallelism,
            Iterations = Iterations,
            MemorySize = Memory,
        };

        var hash = argon2.GetBytes(HashSize);

        return string.Join(
            "$",
            "argon2id",
            Iterations,
            Memory,
            Parallelism,
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash)
        );
    }

    /// <summary>
    /// Verifica se a senha fornecida corresponde ao hash armazenado.
    /// </summary>
    /// <param name="password">A senha em texto plano para verificação.</param>
    /// <param name="hashedPassword">O hash da senha armazenado para comparação.</param>
    /// <returns>Verdadeiro se a senha for válida; caso contrário, falso.</returns>
    public bool VerifyPassword(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split("$");
        if (parts.Length != 6)
            return false;
        if (parts[0] != "argon2id")
            return false;

        try
        {
            var iterations = int.Parse(parts[1]);
            var memory = int.Parse(parts[2]);
            var parallelism = int.Parse(parts[3]);
            var salt = Convert.FromBase64String(parts[4]);
            var expectedHash = Convert.FromBase64String(parts[5]);

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = parallelism,
                Iterations = iterations,
                MemorySize = memory,
            };

            var computedHash = argon2.GetBytes(HashSize);

            return CryptographicOperations.FixedTimeEquals(expectedHash, computedHash);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Verifica se o hash da senha precisa ser atualizado devido a mudanças nos parâmetros de segurança.
    /// </summary>
    /// <param name="hashedPassword">O hash da senha a ser verificado.</param>
    /// <returns>Verdadeiro se os parâmetros do hash forem diferentes dos atuais; caso contrário, falso.</returns>
    public bool NeedsRehash(string hashedPassword)
    {
        var parts = hashedPassword.Split("$");
        if (parts.Length != 6)
            return false;
        if (parts[0] != "argon2id")
            return false;

        var iterations = int.Parse(parts[1]);
        var memory = int.Parse(parts[2]);
        var paralelism = int.Parse(parts[3]);

        return iterations < Iterations
            || iterations > Iterations
            || memory < Memory
            || memory > Memory
            || paralelism < Parallelism
            || paralelism > Parallelism;
    }
}
