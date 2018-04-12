using System;
using UnityEngine;

[Serializable]
public class ContinuousPayload : EMSPayload
{
    public float distance;
    public float maxDistance;

    public ContinuousPayload(float distance, float maxDistance, Side side) : base(side)
    {
        this.distance = distance;
        this.maxDistance = maxDistance;
    }
}