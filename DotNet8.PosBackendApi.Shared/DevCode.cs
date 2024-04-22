using Effortless.Net.Encryption;

namespace DotNet8.PosBackendApi.Shared;

public static class DevCode
{
    public static string ToHash(this string password, string sharedKey)
    {
        return  Hash.Create(HashType.SHA256, password, sharedKey, false);
    }
}
