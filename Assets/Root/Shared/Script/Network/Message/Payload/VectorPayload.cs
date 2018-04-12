using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VectorPayload : BasicPayload
{
    public Vector3 vector;
    public string source;

    public VectorPayload(Vector3 vector, string src = "UnknownSrc")
    {
        this.vector = vector;
        this.source = src;
    }
}