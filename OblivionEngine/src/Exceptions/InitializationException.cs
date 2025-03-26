namespace OblivionEngine.Exceptions;

public class InitializationException : System.Exception
{
    public InitializationException() : base() { }
    public InitializationException(string message) : base(message) { }
    public InitializationException(string message, System.Exception inner) : base(message, inner) { }

    protected InitializationException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}