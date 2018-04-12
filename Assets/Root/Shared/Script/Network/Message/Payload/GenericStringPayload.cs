using System;

[Serializable]
public class GenericStringPayload : BasicPayload
{
    public string payload;
    public string source;

    public GenericStringPayload(string message, string src = "UnknownSrc")
    {
        payload = message;
        source = src;
    }
}