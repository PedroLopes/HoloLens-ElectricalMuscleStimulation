using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ToggleSwitchPayload : EMSPayload
{
    public float angle;
    public float startAngle;

    public ToggleSwitchPayload(float angle, float startAngle, Side side) : base(side)
    {
        this.angle = angle;
        this.startAngle = startAngle;
    }
}