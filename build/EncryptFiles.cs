using Nuke.Common.IO;

namespace build;

public sealed partial class Build
{
    private static readonly AbsolutePath TotalEncryptOutputPath = RootDirectory / "OblivionEngine" / "resources" / "Oblivion.bin";
    
    private void EncryptOutput()
    {
        string key = File.ReadAllText("EncryptionKey.bin");
        byte[] keyBytes = StringToByteArrayFastest(key);
        
        Console.WriteLine(keyBytes.Length.ToString());
        
        using FileStream outputStream = new FileStream(TotalEncryptOutputPath, FileMode.Create, FileAccess.Write);

        byte[] currData = File.ReadAllBytes(TotalOutputPath);

        byte[] finalData = EncryptOrDecrypt(currData, keyBytes);
        outputStream.Write(keyBytes);
        outputStream.Write(finalData);
    }
    
    public static byte[] EncryptOrDecrypt(byte[] text, byte[] key)
    {
        byte[] xor = new byte[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            xor[i] = (byte)(text[i] ^ key[i % key.Length]);
        }
        return xor;
    }
    
    public static byte[] StringToByteArrayFastest(string hex) {
        if (hex.Length % 2 == 1)
            throw new Exception("The binary key cannot have an odd number of digits");
        
        byte[] arr = new byte[hex.Length >> 1];
        
        for (int i = 0; i < hex.Length >> 1; ++i)
        {
            arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
        }
        
        return arr;
    }
    
    public static int GetHexVal(char hex) {
        int val = (int)hex;
        //For uppercase A-F letters:
        //return val - (val < 58 ? 48 : 55);
        //For lowercase a-f letters:
        //return val - (val < 58 ? 48 : 87);
        //Or the two combined, but a bit slower:
        return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
    }
}