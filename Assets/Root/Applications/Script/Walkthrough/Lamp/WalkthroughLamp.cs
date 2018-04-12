using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkthroughLamp : MonoBehaviour {
    public WalkthroughHalo halo;

    public float scalePercentage = 100;
    public float minScaleX = 1f;
    public float maxScaleX = 10f;
    public float minScaleY = 1f;
    public float maxScaleY = 10f;
    public Color lowColor;
    public Color highColor;

    // Use this for initialization
    void Start () {
        halo.gameObject.SetActive(false);
	}

    public void SetHaloSize(float t)
    {
        scalePercentage = t * 100 ;
        Vector3 scale = halo.transform.localScale;
        scale.x = Mathf.Lerp(minScaleX, maxScaleX, t);
        scale.y = Mathf.Lerp(minScaleY, maxScaleY, t);
        halo.transform.localScale = scale;
        
    }
    [Range(0,1)]
    public float brightness;
    public void SetHaloColor(float t)
    {
        //Color color = Color.HSVToRGB(60f / 255f, t, 1);
        Color color =Color.Lerp(lowColor, highColor, t);
        float h, s, v;

        Color.RGBToHSV(color, out h, out s, out v);

        color = Color.HSVToRGB(h, s, brightness);

        halo.ChangeColor(color);
    }   

    // Update is called once per frame
    void Update () {
        //SetHaloSize(scalePercentage / 100f);
	}
}
