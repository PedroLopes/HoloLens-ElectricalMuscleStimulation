using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    public float alpha;
    public float lerpFactor = 1;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        alpha = Mathf.Lerp(alpha, 0f, lerpFactor * Time.deltaTime);
        if (alpha < 0.1)
            alpha = 0;
        GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 1, 1, alpha));
    }
}