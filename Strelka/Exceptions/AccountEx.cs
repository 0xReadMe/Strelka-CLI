namespace Strelka_DLL;

[Serializable]
public class AccountEx : Exception
{
    public AccountEx()
    { }
    public AccountEx(string message)
        : base(message)
    { }
    public AccountEx(string message, Exception innerException)
        : base(message, innerException)
    { }
}