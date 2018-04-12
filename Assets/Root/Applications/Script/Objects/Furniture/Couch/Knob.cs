using UnityEngine;
using System.Collections;

public class Knob : MonoBehaviour
{
    private Transform armRest;

    // Use this for initialization
    private void Start()
    {
        armRest = Util.GetSiblingTransformByName(transform.parent.gameObject, "ArmRestL");
    }

    // Update is called once per frame
    private void Update()
    {
        if (armRest)
        {
            transform.position = armRest.position;
        }
    }
}