using UnityEngine;
using UnityEngine.VR.WSA.Input;

public enum GazeMode
{
    Select = 1,
    Delete = 2,
    Modify = 3,
    Place = 4,
}

public class GazeGestureManager : MonoBehaviour
{
    public static GazeGestureManager Instance { get; private set; }

    // Represents the hologram that is currently being gazed at.
    public GameObject FocusedObject;

    public GestureRecognizer recognizer;
    public bool IsNavigating { get; private set; }
    public Vector3 NavigationPosition { get; private set; }
    public GameObject lastSelected;
    public GazeMode mode = GazeMode.Select;
    private GameObject cursor;

    private SendMessageOptions options = SendMessageOptions.DontRequireReceiver;

    public void setMode(GazeMode mode)
    {
        this.mode = mode;
        MeshRenderer renderer = cursor.GetComponent<MeshRenderer>();
        switch (mode)
        {
            case GazeMode.Select:
                renderer.material.SetColor("_Color", Color.blue);
                break;

            case GazeMode.Place:
                renderer.material.SetColor("_Color", Color.cyan);
                break;

            case GazeMode.Modify:
                renderer.material.SetColor("_Color", Color.green);
                break;

            case GazeMode.Delete:
                renderer.material.SetColor("_Color", Color.red);
                break;
        }
    }

    // Use this for initialization
    private void Start()
    {
        Instance = this;

        cursor = GameObject.Find("Cursor");

        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(
            GestureSettings.Tap |
            GestureSettings.NavigationX);

        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            OnTap();
        };

        // 2.b: Register for the NavigationStartedEvent with the NavigationRecognizer_NavigationStartedEvent function.
        recognizer.NavigationStartedEvent += NavigationRecognizer_NavigationStartedEvent;
        // 2.b: Register for the NavigationUpdatedEvent with the NavigationRecognizer_NavigationUpdatedEvent function.
        recognizer.NavigationUpdatedEvent += NavigationRecognizer_NavigationUpdatedEvent;
        // 2.b: Register for the NavigationCompletedEvent with the NavigationRecognizer_NavigationCompletedEvent function.
        recognizer.NavigationCompletedEvent += NavigationRecognizer_NavigationCompletedEvent;
        // 2.b: Register for the NavigationCanceledEvent with the NavigationRecognizer_NavigationCanceledEvent function.
        recognizer.NavigationCanceledEvent += NavigationRecognizer_NavigationCanceledEvent;

        recognizer.StartCapturingGestures();
    }

    public void OnTap()
    {
        // Send an OnSelect message to the focused object and its ancestors.
        if (FocusedObject != null)
        {
            switch (mode)
            {
                case GazeMode.Select:
                    FocusedObject.SendMessageUpwards("OnSelect", null, options);
                    break;

                case GazeMode.Place:
                    if (lastSelected)
                        lastSelected.SendMessageUpwards("FinishPlacing", null, options);
                    break;

                case GazeMode.Modify:
                    FocusedObject.SendMessageUpwards("OnModify", null, options);
                    break;

                case GazeMode.Delete:
                    FocusedObject.SendMessageUpwards("Delete", null, options);
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        // 2.b: Unregister events on the NavigationRecognizer.
        recognizer.NavigationStartedEvent -= NavigationRecognizer_NavigationStartedEvent;
        recognizer.NavigationUpdatedEvent -= NavigationRecognizer_NavigationUpdatedEvent;
        recognizer.NavigationCompletedEvent -= NavigationRecognizer_NavigationCompletedEvent;
        recognizer.NavigationCanceledEvent -= NavigationRecognizer_NavigationCanceledEvent;
    }

    private void NavigationRecognizer_NavigationStartedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        // 2.b: Set IsNavigating to be true.
        IsNavigating = true;

        // 2.b: Set NavigationPosition to be relativePosition.
        NavigationPosition = relativePosition;
        if (FocusedObject != null)
        {
            FocusedObject.SendMessageUpwards("OnRotationStart", null, options);
        }
    }

    private void NavigationRecognizer_NavigationUpdatedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        // 2.b: Set IsNavigating to be true.
        IsNavigating = true;

        // 2.b: Set NavigationPosition to be relativePosition.
        NavigationPosition = relativePosition;
        if (FocusedObject != null)
        {
            FocusedObject.SendMessageUpwards("OnRotationUpdate", null, options);
        }
    }

    private void NavigationRecognizer_NavigationCompletedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        // 2.b: Set IsNavigating to be false.
        IsNavigating = false;
        if (FocusedObject != null)
        {
            FocusedObject.SendMessageUpwards("OnRotationEnd", null, options);
        }
    }

    private void NavigationRecognizer_NavigationCanceledEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        // 2.b: Set IsNavigating to be false.
        IsNavigating = false;
        if (FocusedObject != null)
        {
            FocusedObject.SendMessageUpwards("OnRotationEnd");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Figure out which hologram is focused this frame.
        GameObject oldFocusObject = FocusedObject;

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            // If the raycast hit a hologram, use that as the focused object.
            FocusedObject = hitInfo.collider.gameObject;
        }
        else
        {
            // If the raycast did not hit a hologram, clear the focused object.
            //FocusedObject = null;
        }

        // If the focused object changed this frame,
        // start detecting fresh gestures again.
        if ((FocusedObject != oldFocusObject))
        {
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            OnTap();
        }
#endif
    }
}