using UnityEngine;
using System.Collections;

public class Hint : MonoBehaviour
{
    public GameObject target;
#if UNITY_EDITOR
    public bool update = false;
#endif

    protected void Update()
    {
        if (target)
        {
            transform.LookAt(target.transform);
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(target.activeInHierarchy);
            }

            transform.localScale = new Vector3(
                1,
                1,
                Vector3.Distance(transform.position, target.transform.position)
            );
        }
    }

#if UNITY_EDITOR

    protected void OnValidate()
    {
        if (update)
            Update();
    }

#endif
}