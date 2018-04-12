using UnityEngine;
using System.Collections;
using System;

public class CouchScript : MonoBehaviour
{
    public float rotationSpeed = 1;
    public MeshRenderer[] renderers;

    private Rigidbody rigidBody;

    private DateTime kinematicTime;
    private bool changeToKinematic;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        if (renderers.Length == 0)
        {
            renderers = GetComponentsInChildren<MeshRenderer>();
        }
    }

    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        if (changeToKinematic && DateTime.Now > kinematicTime)
        {
            rigidBody.isKinematic = true;
            changeToKinematic = false;
        }

        if (transform.position.magnitude > 50)
        {// we probably fell through the floor
            GameObject.Destroy(gameObject);
        }
    }

    private void OnCollisionEnter()
    {
        changeToKinematic = true;
        kinematicTime = DateTime.Now.AddSeconds(3);
    }

    public void ChangeColor(Color color)
    {
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material.color = color;
        }
    }
}