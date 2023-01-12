namespace Strelka_DLL;

[Serializable]
public class ParserEx : Exception
{
    public ParserEx()
    { }
    public ParserEx(string message)
        : base(message)
    { }
    public ParserEx(string message, Exception innerException)
        : base(message, innerException)
    { }
}