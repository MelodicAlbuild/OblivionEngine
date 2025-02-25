using System.Runtime.InteropServices;

namespace EndianBinarySystem;

public static class FileHeaderExtensions
{
    public static byte[] ToBytes(this FileHeaderCS structObj)
    {
        int size = Marshal.SizeOf(structObj);
        byte[] arr = new byte[size];
        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(structObj, ptr, false);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }
    
    public static byte[] ToBytes(this FileHeaderNoCS structObj)
    {
        int size = Marshal.SizeOf(structObj);
        byte[] arr = new byte[size];
        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(structObj, ptr, false);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }
}