using System.Security.Cryptography;
using EndianBinarySystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using Nuke.Common.IO;

namespace build;

public sealed partial class Build
{
    private static readonly AbsolutePath LocationPath = RootDirectory / "OblivionEngine" / "resources" / "locations";
    private static readonly AbsolutePath LocationOutputPath = RootDirectory / "OblivionEngine" / "resources" / "locations" / "bin";
    private static readonly AbsolutePath LocationCompressedPath = RootDirectory / "OblivionEngine" / "resources" / "locations" / "Locations.bin";
    
    private void CleanLocation()
    {
        static void DeleteFiles(AbsolutePath path)
        {
            foreach (string file in path.GlobFiles("*.bin")) File.Delete(file);
        }

        static void DeleteFile(string file)
        {
            if (File.Exists(file)) File.Delete(file);
        }
        
        DeleteFiles(LocationOutputPath);
        
        DeleteFile(LocationCompressedPath);
    }

    private void BuildLocation()
    {
        if (!Directory.Exists(LocationOutputPath))
        {
            Directory.CreateDirectory(LocationOutputPath);
        }
        
        foreach (AbsolutePath absolutePath in LocationPath.GetFiles())
        {
            var json = JArray.Parse(File.ReadAllText(absolutePath));
            var ms = new MemoryStream();
            using (var writer = new BsonDataWriter(ms))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, json);
            }
            
            using(var w = new EndianBinaryWriter(File.Create(Path.Combine(LocationOutputPath, $"{absolutePath.NameWithoutExtension}.bin"))))
            {
                w.Write(ms.ToArray());
            }
        }
    }
}