using UnityEngine;

public class GateScript : ButtonReceiver
{
    private Vector3 startScale;
    public bool opening;
    public float speed = 0.1f;

    //public override void Reset()
    //{
    //    opening = false;
    //    transform.position = startPos;
    //}

    // Use this for initialization
    private void Start()
    {
        startScale = transform.localScale;
    }

    // Update is called once per frame
    private void Update()
    {

        Vector3 scale = transform.localScale;
        if (opening)
        {
            scale.y = Mathf.Clamp(scale.y - speed * Time.deltaTime, 0, startScale.y);
        }
        else
        {
            scale.y = Mathf.Clamp(scale.y + speed * Time.deltaTime, 0, startScale.y);
        }

        transform.localScale = scale;
    }

    public void ToggleOpen()
    {
        opening = !opening;
        GetComponent<AudioSource>().Play();
    }

    public override void onPress(string args)
    {
        ToggleOpen();
    }
}