using Microsoft.Win32;
using OblivionEngine.Exceptions;

namespace OblivionEngine.Init;

public class SysInit
{
    internal SysInit()
    {
        Init();
    }

    static SysInit()
    {
    }
    
    private static readonly SysInit _instance = new SysInit();
    public static SysInit Instance { get { return _instance; } }

    public string EncryptKey = "";
    public bool Initialized = false;
    
    private void Init()
    {
        Console.WriteLine("Initializing Oblivion Engine...");
        
        Initialized = true;

        if (Initialized)
        {
            Console.WriteLine("Oblivion Engine Initialized!");
        }
        else
        {
            throw new InitializationException("Registry failed to initialize");
        }
    }
}