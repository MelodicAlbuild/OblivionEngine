namespace EndianBinarySystem;

public interface IBinaryAttribute<T>
{
    T Value { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class BinaryIgnoreAttribute : Attribute, IBinaryAttribute<bool>
{
    public BinaryIgnoreAttribute(bool ignore = true)
    {
        Value = ignore;
    }

    public bool Value { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class BinaryBooleanSizeAttribute : Attribute, IBinaryAttribute<BooleanSize>
{
    public BinaryBooleanSizeAttribute(BooleanSize booleanSize = BooleanSize.U8)
    {
        if (booleanSize >= BooleanSize.MAX)
            throw new ArgumentOutOfRangeException(
                $"{nameof(BinaryBooleanSizeAttribute)} cannot be created with a size of {booleanSize}.");
        Value = booleanSize;
    }

    public BooleanSize Value { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class BinaryEncodingAttribute : Attribute, IBinaryAttribute<EncodingType>
{
    public BinaryEncodingAttribute(EncodingType encodingType = EncodingType.ASCII)
    {
        if (encodingType >= EncodingType.MAX)
            throw new ArgumentOutOfRangeException(
                $"{nameof(BinaryEncodingAttribute)} cannot be created with a size of {encodingType}.");
        Value = encodingType;
    }

    public EncodingType Value { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class BinaryStringNullTerminatedAttribute : Attribute, IBinaryAttribute<bool>
{
    public BinaryStringNullTerminatedAttribute(bool nullTerminated = true)
    {
        Value = nullTerminated;
    }

    public bool Value { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class BinaryArrayFixedLengthAttribute : Attribute, IBinaryAttribute<int>
{
    public BinaryArrayFixedLengthAttribute(int length)
    {
        if (length < 0)
            throw new ArgumentOutOfRangeException(
                $"{nameof(BinaryArrayFixedLengthAttribute)} cannot be created with a length of {length}. Length must be 0 or greater.");
        Value = length;
    }

    public int Value { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class BinaryArrayVariableLengthAttribute : Attribute, IBinaryAttribute<string>
{
    public BinaryArrayVariableLengthAttribute(string anchor)
    {
        Value = anchor;
    }

    public string Value { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class BinaryStringFixedLengthAttribute : Attribute, IBinaryAttribute<int>
{
    public BinaryStringFixedLengthAttribute(int length)
    {
        if (length <= 0)
            throw new ArgumentOutOfRangeException(
                $"{nameof(BinaryStringFixedLengthAttribute)} cannot be created with a length of {length}. Length must be 0 or greater.");
        Value = length;
    }

    public int Value { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class BinaryStringVariableLengthAttribute : Attribute, IBinaryAttribute<string>
{
    public BinaryStringVariableLengthAttribute(string anchor)
    {
        Value = anchor;
    }

    public string Value { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class BinaryStringTrimNullTerminatorsAttribute : Attribute, IBinaryAttribute<bool>
{
    public BinaryStringTrimNullTerminatorsAttribute(bool trim = true)
    {
        Value = trim;
    }

    public bool Value { get; }
}