using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkthroughHalo : MonoBehaviour {

    public Renderer childRenderer;

    public int hue;
	// Use this for initialization
	void Start () {
		
	}

    public void ChangeColor(Color color)
    {
        //childRenderer.material.EnableKeyword("_EMISSION")
        childRenderer.material.EnableKeyword("_TintColor");
        childRenderer.material.SetColor("_TintColor", color);
    }
	
	// Update is called once per frame
	void Update () {
        //Color32 color = Color.HSVToRGB(hue/255f, 1, 1);
        
        //ChangeColor(color);
	}
}
