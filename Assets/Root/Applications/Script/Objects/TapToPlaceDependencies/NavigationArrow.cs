using UnityEngine;
using System.Collections;

public class NavigationArrow : MonoBehaviour
{
    public GameObject target;
    private float minDistance = 1;
    private Transform sprite;

    private static NavigationArrow _instance;

    public static NavigationArrow Get()
    {
        return _instance;
    }

    private void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    private void Start()
    {
        sprite = transform.GetChild(0);
    }

    // Update is called once per frame
    private void Update()
    {
        bool show = false;
        if (target && target.activeSelf)
        {
            // project onto camera plane
            Vector3 pos = Camera.main.transform.InverseTransformPoint(target.transform.position);
            //minDistance = Mathf.Abs(pos.z) + 0.1f;
            pos.z = 0;

            minDistance = Mathf.Min(2, Vector3.Distance(Camera.main.transform.position, target.transform.position) / 5);

            if (pos.magnitude > minDistance)
            {
                float angle = Mathf.Rad2Deg * Mathf.Acos(pos.y / pos.magnitude);

                angle *= -Mathf.Sign(pos.x);

                transform.localEulerAngles = new Vector3(0, 0, angle);
                show = true;
            }
        }

        sprite.gameObject.SetActive(show);
    }
}