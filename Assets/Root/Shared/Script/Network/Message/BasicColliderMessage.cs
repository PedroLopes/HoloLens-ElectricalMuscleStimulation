using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BasicColliderMessage
{
    protected float distance;
    protected float maxDistance;
    protected virtual string type { get { return ""; } }
    protected string side;
}