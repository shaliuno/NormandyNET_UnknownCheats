internal class XOREncryption
{
    private static bool doEncrypt = true;

    internal static string encryptDecrypt(string input)
    {
        char[] key = { 'K', 'C', 'Q' };
        char[] output = new char[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            output[i] = (char)(input[i] ^ key[i % key.Length]);
        }

        return new string(output);
    }

    internal static char[] key;

    internal static byte[] encryptDecrypt(byte[] input)
    {
        if (doEncrypt)
        {
            byte[] output = new byte[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                output[i] = (byte)(input[i] ^ key[i % key.Length]);
            }
            return output;
        }
        return input;
    }

    [System.Reflection.ObfuscationAttribute(Feature = "Virtualization", Exclude = false)]
    internal static char[] GetKey_VMP()
    {
        return new char[] { '0', 'K', 'f', 't', 'i', 'O', 'f', '1', '2', 'u', 'N', 'x', 'n', 'm', 't', 'Y' };
    }

    [System.Reflection.ObfuscationAttribute(Feature = "Virtualization", Exclude = false)]
    internal static void Init()
    {
        key = GetKey_VMP();
    }
}