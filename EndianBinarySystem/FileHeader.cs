using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace EndianBinarySystem;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FileHeaderCS
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

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FileHeaderNoCS
{
    public int FileSize;
    public string FileName;
    public string FullName;
}