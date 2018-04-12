using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControl : MonoBehaviour
{
    public GameObject sizeWrapper;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            sizeWrapper.SetActive(!sizeWrapper.activeSelf);
        }
    }

    public void Enable()
    {
        sizeWrapper.SetActive(true);
    }

    public void Disable()
    {
        sizeWrapper.SetActive(false);
    }
}