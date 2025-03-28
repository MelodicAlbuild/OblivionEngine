namespace OblivionEngine.Exceptions;

public class NpcLoadException : System.Exception
{
    public NpcLoadException() : base() { }
    public NpcLoadException(string message) : base(message) { }
    public NpcLoadException(string message, System.Exception inner) : base(message, inner) { }

    protected NpcLoadException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}