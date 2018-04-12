using System.Collections;
using System.Text;
using UnityEngine;

public class TCPTest : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private float wait = 0;

    public string msg = "test";

    private void Update()
    {
        if (wait <= 0)
        {
            Debug.Log(msg);

            TCPClient.Instance.SendString(msg);
            wait = 1.0f;
        }
        else
        {
            wait -= Time.deltaTime;
        }
    }
}