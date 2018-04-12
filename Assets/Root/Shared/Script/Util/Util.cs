using UnityEngine;
using System.Collections;

public class Util
{
    public static GameObject GetParentByName(GameObject obj, string name, bool fuzzy = true)
    {
        GameObject result = null;
        Transform current = obj.transform;

        while (current.parent)
        {
            if (fuzzy && current.parent.name.Contains(name)
                || current.parent.name == name)
            {
                result = current.parent.gameObject;
                break;
            }
            current = current.parent;
        }

        return result;
    }

    public static GameObject GetSiblingByName(GameObject obj, string name)
    {
        GameObject result = null;
        Transform transform = GetSiblingTransformByName(obj, name);
        if (transform != null)
        {
            result = transform.gameObject;
        }

        return result;
    }

    public static Transform GetSiblingTransformByName(GameObject obj, string name)
    {
        foreach (Transform trans in obj.transform.parent.GetComponentsInChildren<Transform>(true))
        {
            if (trans.gameObject.name == name)
                return trans;
        }
        return null;
    }

    public static void SetLayerOnAll(GameObject obj, int layer)
    {
        foreach (Transform trans in obj.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layer;
        }
    }
}