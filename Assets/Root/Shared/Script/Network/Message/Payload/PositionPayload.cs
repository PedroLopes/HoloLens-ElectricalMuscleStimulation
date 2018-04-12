using System;

[Serializable]
public class PositionPayload : BasicPayload
{
    public float position;
    public string source;

    public PositionPayload(float pos, string src = "UnknownSrc")
    {
        position = pos;
        source = src;
    }
}