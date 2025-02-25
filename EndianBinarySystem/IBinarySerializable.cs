namespace EndianBinarySystem;

public interface IBinarySerializable
{
    void Read(EndianBinaryReader r);
    void Write(EndianBinaryWriter w);
}