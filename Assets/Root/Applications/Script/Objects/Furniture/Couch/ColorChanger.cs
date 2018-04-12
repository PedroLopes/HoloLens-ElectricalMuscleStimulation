using UnityEngine;
using System.Collections;

public class ColorChanger : MonoBehaviour
{
    public GameObject couch;

    // scale of rail mesh
    private float max = .5f;

    // Use this for initialization
    private void Start()
    {
        if (!couch)
        {
            couch = GameObject.Find("CouchGeometry");
        }
    }

    protected float lastHue = 0f;

    // Update is called once per frame
    private void Update()
    {
        float hue = (transform.localPosition.z + max / 2) / max;
        hue = 1 - hue;
        if (hue != lastHue)
        {
            Color color = Color.HSVToRGB(hue, 1, 1);
            couch.SendMessage("ChangeColor", color);
            lastHue = hue;
        }
        //print(hue);
    }
}