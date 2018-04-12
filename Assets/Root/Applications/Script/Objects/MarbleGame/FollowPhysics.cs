using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FollowPhysics : MonoBehaviour
{
    public Transform target;
    protected Rigidbody rigidBody;
    protected Vector3 offset;

    // Use this for initialization
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        offset = target.InverseTransformPoint(transform.position);
    }

    // Update is called once per frame
    private void Update()
    {
        rigidBody.MovePosition(target.TransformPoint(offset));
        rigidBody.MoveRotation(target.rotation);
    }
}