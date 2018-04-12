using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHandler : MonoBehaviour
{
    public GameObject[] objects;
    protected Dictionary<int, Vector3> positions;
    protected Dictionary<int, Quaternion> rotations;
    
    // Use this for initialization
    public void Save () {
        positions = new Dictionary<int, Vector3>();
        rotations = new Dictionary<int, Quaternion>();

        foreach (var obj in objects)
        {
            positions.Add(obj.GetInstanceID(), obj.transform.position);
            rotations.Add(obj.GetInstanceID(), obj.transform.rotation);
        }
	}

    public void Load()
    {
        foreach (var obj in objects)
        {
            obj.transform.position = positions[obj.GetInstanceID()];
            obj.transform.rotation = rotations[obj.GetInstanceID()];
        }
    }
}
