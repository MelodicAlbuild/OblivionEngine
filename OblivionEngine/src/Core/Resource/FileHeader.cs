using System.Runtime.InteropServices;

namespace OblivionEngine.Core.Resource;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FileHeader
{
    public int FileSize;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string FileName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string FullName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string Checksum;
    public int isContainerFile;
    public int totalFiles;
}