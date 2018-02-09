using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CameraManager Script
 * 
 * This is the CameraManager script, it is used to control the camera inside of the game.
 * This script handles the movement of the camera between player moves, and moving it
 * to the center of the screen between the turns.
 * 
 * Anything related to the camera should be handled by this script, to makes sure that
 * we keep the project clean and neat; also avoiding any duplicated code.
 * 
 */

public class CameraManager : MonoBehaviour {

    public static CameraManager CMInstance;

    [Header("Camera Settings")]
    [Tooltip("Reference to the camera used in the scene, this will be the camera that will move when players change turns.")]
    public Camera cameraObject;
    [Tooltip("This is how long (in seconds) it will take the camera to move from position A to position B.")]
    public float cameraPanLength;
    [Tooltip("This is how long (in seconds) it will take the camera to scale from size A to size B.")]
    public float cameraZoomLength;

    // ANY DEBUG VARIABLES BELOW; HIDE WHEN DONE.. pls.
    [Header("Debug Stuff?")]
    [SerializeField] private Vector3 intermissionPosition;
    [SerializeField] private Vector3 desiredPosition;
    [SerializeField] private float desiredSize;

    private Vector3 startPosition;
    private float cameraStartSize;

    private float timeStartedLerping;

    private bool cameraMoving;
    private bool cameraResize;

    private void Awake()
    {
        if (CMInstance == null)
            CMInstance = this;
        else if (CMInstance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
    }

    private void Update ()
    {
        // Wait for player input to start lerp.
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetNewCameraPosition(new Vector3(-25, 0, -10));
            SetNewCameraSize(10.0f);
            StartLerping();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetNewCameraPosition(new Vector3(0, 0, -10));
            SetNewCameraSize(15.0f);
            StartLerping();
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            SetNewCameraPosition(new Vector3(25, 0, -10));
            SetNewCameraSize(10.0f);
            StartLerping();
        }
    }

#region CAMERA MOVEMENT
    private void StartLerping()
    {
        startPosition = cameraObject.gameObject.transform.position;
        cameraStartSize = cameraObject.orthographicSize;

        cameraMoving = true;
        cameraResize = true;

        timeStartedLerping = Time.time;
    }

    private void FixedUpdate()
    {
        if (cameraMoving)
        {
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageComplete = timeSinceStarted / cameraPanLength;

            cameraObject.transform.position = Vector3.Lerp(startPosition, desiredPosition, percentageComplete);

            if (percentageComplete >= 1.0f)
                cameraMoving = false;
        }

        if (cameraResize)
        {
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageComplete = timeSinceStarted / cameraZoomLength;

            cameraObject.orthographicSize = Mathf.SmoothStep(cameraStartSize, desiredSize, percentageComplete);

            if (percentageComplete >= 1.0f)
                cameraResize = false;
        }
    }

    public void SetNewCameraPosition(Vector3 newPosition)
    {
        desiredPosition = newPosition;
    }

    public void SetNewCameraSize(float newSize)
    {
        desiredSize = newSize;
    }
}
#endregion