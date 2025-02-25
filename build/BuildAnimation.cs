using EndianBinarySystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using Nuke.Common.IO;

namespace build;

public sealed partial class Build
{
    private static readonly AbsolutePath AnimationPath = RootDirectory / "OblivionEngine" / "resources" / "animations";
    private static readonly AbsolutePath AnimationOutputPath = RootDirectory / "OblivionEngine" / "resources" / "animations" / "bin";
    private static readonly AbsolutePath AnimationCompressPath = RootDirectory / "OblivionEngine" / "resources" / "animations" / "Animations.bin";
    
    private void CleanAnimation()
    {
        static void DeleteFiles(AbsolutePath path)
        {
            foreach (string file in path.GlobFiles("*.bin")) File.Delete(file);
        }

        static void DeleteFile(string file)
        {
            if (File.Exists(file)) File.Delete(file);
        }
        
        DeleteFiles(AnimationOutputPath);
        
        DeleteFile(AnimationCompressPath);
    }

    private void BuildAnimation()
    {
        if (!Directory.Exists(AnimationOutputPath))
        {
            Directory.CreateDirectory(AnimationOutputPath);
        }
        
        foreach (AbsolutePath absolutePath in AnimationPath.GetFiles())
        {
            var json = JObject.Parse(File.ReadAllText(absolutePath));
            var ms = new MemoryStream();
            using (var writer = new BsonDataWriter(ms))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, json);
            }
            
            using(var w = new EndianBinaryWriter(File.Create(Path.Combine(AnimationOutputPath, $"{absolutePath.NameWithoutExtension}.bin"))))
            {
                w.Write(ms.ToArray());
            }
        }
    }
}