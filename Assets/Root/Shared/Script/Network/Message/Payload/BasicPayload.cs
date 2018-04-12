using System;
using UnityEngine;

[Serializable]
public class BasicPayload
{
    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}