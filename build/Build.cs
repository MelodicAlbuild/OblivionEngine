using Nuke.Common;
using Nuke.Common.IO;

namespace build;

public sealed partial class Build : NukeBuild
{
    public static readonly AbsolutePath AssetPath = RootDirectory / "OblivionEngine" / "resources";

    public Target Clean => _ => _.Before(Compress).Executes(CleanAnimation, CleanLocation);

    public Target Compile => _ => _.After(Clean).Before(Compress).Executes(BuildAnimation, BuildLocation);

    public Target Compress => _ => _.After(Clean).Before(Complete).Executes(CompressAnimation, CompressLocation);

    public Target Complete => _ => _.After(Compress).Executes(CompressAll);

    public static int Main()
    {
        return Execute<Build>(x => x.Clean, x => x.Compile, x => x.Compress, x => x.Complete);
    }
}