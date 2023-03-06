using System.Security.Cryptography;

namespace Bot.Integration.Git.Tests;

public class Hasher
{
    public static string GetSHA256HashString(string filepath)
    {
        using var sha256 = SHA256.Create();
        using var fileStream = File.OpenRead(filepath);
        return ByteArrayToHexString(sha256.ComputeHash(fileStream));
    }    

    private static string ByteArrayToHexString(byte[] bytes)
    {
        string output = string.Empty;
        foreach (var @byte in bytes)
        {
            output += $"{@byte:x2}";
        }
        return output;
    }
}
