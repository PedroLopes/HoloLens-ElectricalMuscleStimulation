using UnityEngine;
using System.Collections;

public class HandlerFactory<T> : MonoBehaviour where T : MonoBehaviour, new()
{
    public static T Get()
    {
        T item = FindObjectOfType<T>();

        if (!item)
        {
            item = new T();
        }

        return item;
    }
}