using System.Security.Cryptography;
using System.Text;
using EndianBinarySystem;
using Nuke.Common.IO;

namespace build;

public sealed partial class Build
{
    private static readonly AbsolutePath TotalOutputPath = RootDirectory / "OblivionEngine" / "resources" / "Oblivion.bin";
    
    private void CompressLocation()
    {
        using FileStream outputStream = new FileStream(LocationCompressedPath, FileMode.Create, FileAccess.Write);
        
        byte[] containerHeader = BuildContainerFileHeader("Locations.bin", LocationOutputPath.GetFiles().Count());
        outputStream.Write(containerHeader, 0, containerHeader.Length);
        
        foreach (AbsolutePath absolutePath in LocationOutputPath.GetFiles())
        {
            WriteFileWithHeader(absolutePath.ToString(), outputStream);
        }
    }
    
    private void CompressAnimation()
    {
        using FileStream outputStream = new FileStream(AnimationCompressPath, FileMode.Create, FileAccess.Write);
        
        byte[] containerHeader = BuildContainerFileHeader("Animations.bin", AnimationOutputPath.GetFiles().Count());
        outputStream.Write(containerHeader, 0, containerHeader.Length);
        
        foreach (AbsolutePath absolutePath in AnimationOutputPath.GetFiles())
        {
            WriteFileWithHeader(absolutePath.ToString(), outputStream);
        }
    }

    private void CompressAll()
    {
        using FileStream outputStream = new FileStream(TotalOutputPath, FileMode.Create, FileAccess.Write);
        
        byte[] containerHeader = BuildContainerFileHeader("Oblivion.bin", 2);
        outputStream.Write(containerHeader, 0, containerHeader.Length);
        
        WriteFileWithHeader(AnimationCompressPath, outputStream);
        WriteFileWithHeader(LocationCompressedPath, outputStream);
    }
    
    private void WriteFileWithHeader(string inputFile, FileStream outputStream)
    {
        FileInfo fileInfo = new FileInfo(inputFile);
        FileHeaderCS header = new FileHeaderCS
        {
            FileSize = (int)fileInfo.Length,
            FullName = fileInfo.FullName,
            FileName = fileInfo.Name,
            isContainerFile = 0,
            totalFiles = 0
        };

        // Calculate checksum if needed
        header.Checksum = CalculateChecksum(fileInfo.FullName);
        
        byte[] headerBytes = header.ToBytes();
        outputStream.Write(headerBytes, 0, headerBytes.Length);

        using (FileStream inputStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
        {
            inputStream.CopyTo(outputStream);
        }
    }
    
    private void WriteContainerFileWithHeader(string inputFile, FileStream outputStream, int containedFiles)
    {
        FileInfo fileInfo = new FileInfo(inputFile);
        FileHeaderCS header = new FileHeaderCS
        {
            FileSize = (int)fileInfo.Length,
            FullName = fileInfo.FullName,
            FileName = fileInfo.Name,
            isContainerFile = 1,
            totalFiles = containedFiles
        };

        // Calculate checksum if needed
        header.Checksum = CalculateChecksum(fileInfo.FullName);
        
        byte[] headerBytes = header.ToBytes();
        outputStream.Write(headerBytes, 0, headerBytes.Length);
        
        Console.WriteLine(header.FileName + " :: " + header.FileSize);

        using (FileStream inputStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
        {
            inputStream.CopyTo(outputStream);
        }
    }
    
    private void BuildFileHeader(string inputFile, FileStream outputStream)
    {
        FileInfo fileInfo = new FileInfo(inputFile);
        FileHeaderCS header = new FileHeaderCS
        {
            FileSize = (int)fileInfo.Length,
            FullName = fileInfo.FullName,
            FileName = fileInfo.Name
        };

        // Calculate checksum if needed
        header.Checksum = CalculateChecksum(fileInfo.FullName);

        byte[] headerBytes = header.ToBytes();
        outputStream.Write(headerBytes, 0, headerBytes.Length);

        using (FileStream inputStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
        {
            inputStream.CopyTo(outputStream);
        }
    }
    
    public static byte[] BuildFileHeader(string inputFile)
    {
        FileInfo fileInfo = new FileInfo(inputFile);
        FileHeaderNoCS header = new FileHeaderNoCS
        {
            FileSize = (int)fileInfo.Length,
            FullName = fileInfo.FullName,
            FileName = fileInfo.Name
        };

        byte[] headerBytes = header.ToBytes();

        return headerBytes;
    }
    
    public static byte[] BuildContainerFileHeader(string fileName, int containedFiles)
    {
        FileHeaderCS header = new FileHeaderCS
        {
            FileSize = 0,
            FullName = "",
            FileName = fileName,
            isContainerFile = 1,
            totalFiles = containedFiles,
            Checksum = ""
        };

        byte[] headerBytes = header.ToBytes();

        return headerBytes;
    }

    private static string CalculateChecksum(string filePath)
    {
        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(fs);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}