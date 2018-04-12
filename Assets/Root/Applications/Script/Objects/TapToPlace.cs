using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

[RequireComponent(typeof(AudioSource))]
public class TapToPlace : MonoBehaviour
{
    protected bool placing = false;
    public bool onFloor = true;
    public AudioSource rotationStartSound;
    protected int defaultLayer;
    protected bool initialized = false;

    // Called by GazeGestureManager when the user performs a Select gesture
    public virtual void OnSelect()
    {
        GetComponent<AudioSource>().Play();
        // Start placing
        if (!placing)
        {
            StartPlacing();
        }
        // We were already placing so stop now
        else
        {
            FinishPlacing();
        }
    }

    public virtual void OnTap()
    {
        if (placing)
        {
            FinishPlacing();
        }
    }

    public virtual void StartPlacing()
    {
        placing = true;
        Util.SetLayerOnAll(gameObject, LayerMask.NameToLayer("Ignore Raycast"));

        GazeGestureManager.Instance.setMode(GazeMode.Place);
        GazeGestureManager.Instance.lastSelected = gameObject;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody)
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }
    }

    public virtual void Delete()
    {
        GameObject.Destroy(gameObject);
    }

    public virtual void FinishPlacing()
    {
        if (placing)
        {
            placing = false;
            Util.SetLayerOnAll(gameObject, defaultLayer);

            GazeGestureManager.Instance.setMode(GazeMode.Select);
            GazeGestureManager.Instance.lastSelected = null;

            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (rigidbody)
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
            }
        }
    }

    protected virtual void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        if (initialized)
            return;

        defaultLayer = gameObject.layer;

        initialized = true;
    }

    protected virtual void OnModify()
    {
        NavigationArrow.Get().target = gameObject;
        GazeGestureManager.Instance.lastSelected = gameObject;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // If the user is in placing mode,
        // update the placement to match the user's gaze.

        if (placing)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.

            Vector3 normal;
            RaycastHit hitInfo;

            if (GetRayCast(out hitInfo))
            {
                // Move this object's parent object to
                // where the raycast hit the Spatial Mapping mesh.
                transform.position = hitInfo.point;
                normal = hitInfo.normal;
            }
            else
            {
                // If nothing was hit place it in a fixed distance from the camera
                transform.position = Camera.main.transform.TransformPoint(GetDefaultPos());
                normal = onFloor ? Camera.main.transform.up : -Camera.main.transform.forward;
            }

            // Rotate this object to the surface and always facing the camera
            Vector3 left = -Camera.main.transform.right;
            // If parallel to hit
            if ((Vector3.Angle(normal, left) % 180) == 0)
            {
                left = Quaternion.AngleAxis(1, normal) * left;
            }

            Vector3 forward = Vector3.Cross(left, normal);

            transform.rotation = Quaternion.LookRotation(forward, normal)
            ;
        }
    }

    protected virtual bool GetRayCast(out RaycastHit hitInfo)
    {
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        return Physics.Raycast(headPosition, gazeDirection, out hitInfo,
            30.0f, GetRaycastLayerMask());
    }

    protected virtual Vector3 GetDefaultPos()
    {
        float distance = 2.0f;
        return new Vector3(0, -.1f, distance);
    }

    protected int GetRaycastLayerMask()
    {
        //return SpatialMappingManager.Instance.LayerMask;
        return ~(1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("UI"));
    }

    [Tooltip("Rotation max speed controls amount of rotation.")]
    public float RotationSensitivity = 10.0f;

    private Vector3 manipulationPreviousPosition;

    private float rotationFactor;

    private bool isRotating;

    public void OnRotationStart()
    {
        if (rotationStartSound)
            rotationStartSound.Play();
    }

    public void OnRotationUpdate()
    {
        rotationFactor = GazeGestureManager.Instance.NavigationPosition.x * RotationSensitivity;

        // 2.c: transform.Rotate along the Y axis using rotationFactor.
        transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
    }

    public void OnRotationEnd()
    {
        if (rotationStartSound)
            rotationStartSound.Stop();
    }
}