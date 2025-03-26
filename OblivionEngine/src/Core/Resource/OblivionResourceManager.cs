using System.Runtime.InteropServices;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using OblivionEngine.GameSystems;
using OblivionEngine.GameSystems.Anim;
using OblivionEngine.Init;

namespace OblivionEngine.Core.Resource;

public class OblivionResourceManager
{
    public static OblivionResourceManager _instance;

    public OblivionResourceManager()
    {
        _instance = this;
        
        byte[] baseFile = File.ReadAllBytes("resources/Oblivion.bin");
        byte[] keyBytes = baseFile.Take(256).ToArray();
        
        byte[] file = EncryptOrDecrypt(baseFile.Skip(256).ToArray(), keyBytes);

        int bytesRead = 0;
        
        byte[] fheaderBytes = file.Skip(bytesRead).Take(Marshal.SizeOf<FileHeader>()).ToArray();
        bytesRead += fheaderBytes.Length;
        GCHandle fhandle = GCHandle.Alloc(fheaderBytes, GCHandleType.Pinned);
        FileHeader fheader = (FileHeader)Marshal.PtrToStructure(fhandle.AddrOfPinnedObject(), typeof(FileHeader))!;
        fhandle.Free();

        for (int i = 0; i < fheader.totalFiles; i++)
        {
            byte[] headerBytes = file.Skip(bytesRead).Take(Marshal.SizeOf<FileHeader>()).ToArray();
            bytesRead += headerBytes.Length;
            GCHandle handle = GCHandle.Alloc(headerBytes, GCHandleType.Pinned);
            FileHeader header = (FileHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(FileHeader))!;
            handle.Free();

            switch (header.FileName.Split(".")[0])
            {
                case "Animations":
                    Console.WriteLine(header.FileName + " :: " + header.FileSize + " :: " + header.isContainerFile + " :: " + header.totalFiles);

                    List<JSONAnimation> anims = new List<JSONAnimation>();

                    for (int j = 0; j < header.totalFiles; j++)
                    {
                        byte[] newHeaderBytes = file.Skip(bytesRead).Take(Marshal.SizeOf<FileHeader>()).ToArray();
                        bytesRead += newHeaderBytes.Length;
                        GCHandle nHandle = GCHandle.Alloc(newHeaderBytes, GCHandleType.Pinned);
                        FileHeader newHeader = (FileHeader)Marshal.PtrToStructure(nHandle.AddrOfPinnedObject(), typeof(FileHeader))!;
                        nHandle.Free();
                        
                        byte[] dBytes = file.Skip(bytesRead).Take(newHeader.FileSize).ToArray();
                        var reader = new BsonDataReader(new MemoryStream(dBytes));
                        JObject jObject = JObject.Load(reader);
                        
                        anims.Add(jObject.ToObject<JSONAnimation>());
                        
                        bytesRead += newHeader.FileSize;
                    }
                    
                    List<Animation> animations = new List<Animation>();

                    foreach (JSONAnimation jsonAnimation in anims)
                    {
                        animations.Add(new Animation(jsonAnimation));
                    }
                    
                    Game.Instance.GetAnimations().LoadAnimations(animations);
                    break;
                case "Locations":
                    Console.WriteLine(header.FileName + " :: " + header.FileSize + " :: " + header.isContainerFile + " :: " + header.totalFiles);

                    List<JSONLocation> jArray = new List<JSONLocation>();
                    
                    for (int j = 0; j < header.totalFiles; j++)
                    {
                        byte[] newHeaderBytes = file.Skip(bytesRead).Take(Marshal.SizeOf<FileHeader>()).ToArray();
                        bytesRead += newHeaderBytes.Length;
                        GCHandle nHandle = GCHandle.Alloc(newHeaderBytes, GCHandleType.Pinned);
                        FileHeader newHeader = (FileHeader)Marshal.PtrToStructure(nHandle.AddrOfPinnedObject(), typeof(FileHeader))!;
                        nHandle.Free();
                        
                        byte[] dBytes = file.Skip(bytesRead).Take(newHeader.FileSize).ToArray();
                        var reader = new BsonDataReader(new MemoryStream(dBytes));
                        JObject jObject = JObject.Load(reader);
                        
                        foreach (KeyValuePair<string,JToken> keyValuePair in jObject)
                        {
                            jArray.Add(keyValuePair.Value.ToObject<JSONLocation>());
                        }
                        
                        bytesRead += newHeader.FileSize;
                    }
                    
                    Game.Instance.GetLocations().LoadLocations(jArray);
                    break;
            }
        }
    }
    
    private static byte[] EncryptOrDecrypt(byte[] text, byte[] key)
    {
        byte[] xor = new byte[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            xor[i] = (byte)(text[i] ^ key[i % key.Length]);
        }
        return xor;
    }
    
    private static byte[] StringToByteArrayFastest(string hex) {
        if (hex.Length % 2 == 1)
            throw new Exception("The binary key cannot have an odd number of digits");
        
        byte[] arr = new byte[hex.Length >> 1];
        
        for (int i = 0; i < hex.Length >> 1; ++i)
        {
            arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
        }
        
        return arr;
    }
    
    private static int GetHexVal(char hex) {
        int val = (int)hex;
        //For uppercase A-F letters:
        //return val - (val < 58 ? 48 : 55);
        //For lowercase a-f letters:
        //return val - (val < 58 ? 48 : 87);
        //Or the two combined, but a bit slower:
        return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
    }
}