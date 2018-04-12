using UnityEngine;
using System.Collections;

public class ArmRestPosition : MonoBehaviour
{
    private Transform couchLengthTransform;
    public bool isLeft;

    // Use this for initialization
    private void Start()
    {
        couchLengthTransform = Util.GetSiblingTransformByName(gameObject, "CouchLengthWrapper");
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 localPos = transform.localPosition;
        localPos.x = isLeft ?
            -couchLengthTransform.localScale.x * 0.5f :
             couchLengthTransform.localScale.x * 0.5f;

        transform.localPosition = localPos;
    }
}