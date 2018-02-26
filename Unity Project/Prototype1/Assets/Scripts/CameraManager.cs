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
 * we keep the project clean and neat; also avoiding any duplicated code. The code also
 * contains things like the max/min FOV, min/max camera size, and the positions.
 * 
 * The script uses two custom function to set the desiredPosition and desiredSize,
 * this allows them to be set from outside of the script; they can be called from
 * any other script, this allows the script to be more dynamic and used for more than just turns.
 * 
 * ---  ---
 * HOW TO:
 * 
 * In order to currently move a camera, you need to call the function in this order (otherwise it might not work):
 * SetCameraPosition, to set the camera's new position; SetCameraSize, to set the new camera size; StartLerping, to move camera.
 * I will work on making this process simplier, but for now this is the way the functions need to be called.
 * 
 * TODO:
 *  - Add the ability to move the camera around when we are in play mode.
 *  - The player should also be able to zoom the camera in and out (clamp the values)
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
    [Tooltip("This is the minimum ortographic size for the camera, you can't go below this value.")]
    public float minOrthoSize;
    [Tooltip("This is the maximum ortographic size for the camera, you can't go above this value.")]
    public float maxOrthoSize;

    // ANY DEBUG VARIABLES BELOW; HIDE WHEN DONE.. pls.
    [Header("Debug Stuff | Most of this will be removed.")]
    [SerializeField] private Vector3 intermissionPosition;
    [SerializeField] private Vector3 playerOnePosition;
    [SerializeField] private Vector3 playerTwoPosition;
    [SerializeField] private float intermissionSize;
    [SerializeField] private float playerOneSize;
    [SerializeField] private float playerTwoSize;
    [Space]
    [SerializeField] private Vector3 desiredPosition;
    [SerializeField] private float desiredSize;

    private Vector3 startPosition;
    private Vector3 currentPosition;
    private float cameraStartSize;
    private float currentSize;

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
        //SetNewCameraPosition(intermissionPosition);
        //SetNewCameraSize(intermissionSize);
        //StartLerping();
    }

    private void Update ()
    {
        // This is an example of using the camera script, pressing the buttons you can move the camera.
        // This is teh same way you should call the functions whenever you are trying to move the camera.
        // You don't need to set both position and size, but it is advised to in current setup.

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    SetNewCameraPosition(playerOnePosition);
        //    SetNewCameraSize(playerOneSize);
        //    StartLerping();
        //}

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SetNewCameraPosition(intermissionPosition);
        //    SetNewCameraSize(intermissionSize);
        //    StartLerping();
        //}

        //if (Input.GetKeyDown(KeyCode.D)) {
        //    SetNewCameraPosition(playerTwoPosition);
        //    SetNewCameraSize(playerTwoSize);
        //    StartLerping();
        //}
    }

    #region CAMERA MOVEMENT
    public void MoveCamera(Vector3 _newPosition, float _newSize)
    {
        startPosition = cameraObject.gameObject.transform.position;
        cameraStartSize = cameraObject.orthographicSize;

        SetNewCameraPosition(_newPosition);
        SetNewCameraSize(_newSize);

        cameraMoving = true;
        cameraResize = true;

        timeStartedLerping = Time.time;
    }

    private void FixedUpdate()
    {
        currentPosition = cameraObject.transform.position;
        currentSize = cameraObject.orthographicSize;

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
        // Later add a check here, to make sure that the camera never goes out of bounds.
        desiredPosition = newPosition;
    }

    public void SetNewCameraSize(float newSize)
    {
        if ((newSize < maxOrthoSize) && (newSize > minOrthoSize))
            desiredSize = newSize;
        else
            Debug.Log("New camera ortographic size is out of scope, double check the values : " + gameObject.name);
    }
}
#endregion