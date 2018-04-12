using UnityEngine;
using System.Collections;

public class ColorSlider : MonoBehaviour
{
    private Vector3 startPos;
    private string startTag;

    private void Start()
    {
        startTag = gameObject.tag;
        startPos = transform.parent.localPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameObject.CompareTag("Knob")
            && GazeGestureManager.Instance.lastSelected)
        {
            float hue = Mathf.Clamp((transform.parent.localPosition.z + 0.2f) / 0.4f, 0, 1);
            Color color = Color.HSVToRGB(hue, 1, 1);
            GazeGestureManager.Instance.lastSelected.SendMessageUpwards(
                "ChangeColor",
                color,
                SendMessageOptions.DontRequireReceiver
            );
        }
    }

    public void Reset()
    {
        gameObject.tag = startTag;
        transform.parent.localPosition = startPos;
    }
}