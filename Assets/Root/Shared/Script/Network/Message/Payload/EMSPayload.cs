using System;
using UnityEngine;

[Serializable]
public class EMSPayload : BasicPayload
{
    public Side side;

    public EMSPayload(Side side) : base()
    {
        this.side = side;
    }
}