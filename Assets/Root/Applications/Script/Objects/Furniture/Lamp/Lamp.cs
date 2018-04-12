using UnityEngine;
using System.Collections;
using System;

public class Lamp : ButtonReceiver
{
    private GameObject onMesh;
    private GameObject offMesh;

    private bool initialized = false;
    public bool on;

#if UNITY_EDITOR

    // for testing in Unity Editor

    //private void OnValidate()
    //{
    //    ToggleLamp(on);
    //}

#endif

    // Use this for initialization
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (!initialized)
        {
            onMesh = transform.Find("ON").gameObject;
            offMesh = transform.Find("Off").gameObject;
            initialized = true;
        }
    }

    public override void onPress(string args)
    {
        on = !on;
        ToggleLamp(on);
    }

    private void ToggleLamp(bool state)
    {
        Init();
        onMesh.SetActive(state);
        offMesh.SetActive(!state);
    }
}