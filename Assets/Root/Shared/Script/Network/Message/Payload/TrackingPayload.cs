using System;
using UnityEngine;

[Serializable]
public class TrackingPayload : BasicPayload
{
    public string name;
    public Vector3 position;
    public Quaternion rotation;

    public TrackingPayload(String name, Vector3 position, Quaternion rotation) : base()
    {
        this.name = name;
        this.position = position;
        this.rotation = rotation;
    }

    public TrackingPayload(GameObject obj) : base()
    {
        this.name = obj.name;
        this.position = obj.transform.position;
        this.rotation = obj.transform.rotation;
    }
}