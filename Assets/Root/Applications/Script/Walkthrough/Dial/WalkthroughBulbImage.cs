using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkthroughBulbImage : MonoBehaviour {
    public GameObject bulb;

    void Start()
    {
        bulb.SetActive(false);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Knob")
            || other.transform.parent.parent != transform.parent.parent)
            return;

        bulb.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Knob")
            || other.transform.parent.parent != transform.parent.parent)
            return;

        bulb.SetActive(false);
    }
}
