using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Zylo.Application.Abstractions.Authentication;

namespace Zylo.Infrastructure.Authentication;

internal sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100_000;
    private const KeyDerivationPrf Prf = KeyDerivationPrf.HMACSHA256;

    public string Hash(string password)
    {
        string hashedPassword = Convert.ToBase64String(HashPasswordInternal(password));

        return hashedPassword;
    }

    public bool Verify(string password, string passwordHash)
    {
        byte[] decodedHashPasword = Convert.FromBase64String(passwordHash);

        if (decodedHashPasword.Length == 0)
        {
            return false;
        }

        return VerifyPasswordHashInternal(decodedHashPasword, password);
    }

    private static byte[] HashPasswordInternal(string password)
    {
        byte[] salt = GenerateRandomSalt();

        byte[] subKey = KeyDerivation.Pbkdf2(password, salt, Prf, Iterations, HashSize);

        byte[] outputBytes = new byte[salt.Length + subKey.Length];

        Buffer.BlockCopy(salt, 0, outputBytes, 0, salt.Length);

        Buffer.BlockCopy(subKey, 0, outputBytes, salt.Length, subKey.Length);

        return outputBytes;
    }

    private static byte[] GenerateRandomSalt()
    {
        byte[] salt = new byte[SaltSize];

        RandomNumberGenerator.Fill(salt);

        return salt;
    }

    private static bool VerifyPasswordHashInternal(byte[] hashedPassword, string password)
    {
        try
        {
            byte[] salt = new byte[SaltSize];

            Buffer.BlockCopy(hashedPassword, 0, salt, 0, salt.Length);

            int subKeyLength = hashedPassword.Length - salt.Length;

            if (subKeyLength < SaltSize)
            {
                return false;
            }

            byte[] expectedSubkey = new byte[subKeyLength];

            Buffer.BlockCopy(hashedPassword, salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            byte[] actualSubKey = KeyDerivation.Pbkdf2(password, salt, Prf, Iterations, HashSize);

            return CryptographicOperations.FixedTimeEquals(actualSubKey, expectedSubkey);
        }
        catch
        {
            return false;
        }
    }
}
