using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CameraManager Script by Daniel Pokladek.
 * 
 * This is the CameraManager script, it is used to control and move the camera in the game; it can be used outside of the round too (for example in the menu).
 * The script will handle the position of the camera, and the ortographic size of the camera; it's main task is to move between player turns and intermission.
 * It can be used to do any type of movement of the camera, you just need to pass it variables; it does not support rotation because we don't need it.
 * 
 * When working on this script, make sure to have anything related to camera movement inside this script, this makes sure we keep the code nice and clean.
 * Try to avoid making more camera scripts unless its completly necessary; let the managers know first before modyfing this core script.
 * 
 * In order to use this script, you need to call the MoveCamera(desiredPosition, desiredSize) function which has to be accessed through the CameraManager.
 * CameraManager.CMInstance.MoveCamera(playerOne, playerOneSize) : this will move the camera to playerOne position, and change the size of camera to playerOneSize.
 * 
 * TODO:
 *  - Give the player option to move the camera around by dragging finger around the screen.
 *  - Give the player option to zoom in and out the camera (pinch the screen).
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

    private Vector3 startPosition;
    private float cameraStartSize;
    //private Vector3 currentPosition;
    //private float currentSize;

    private Vector3 desiredPosition;
    private float desiredSize;

    private float timeStartedLerping;

    // We hide them in inspector, so that we can stil access them from outside.
    // And they don't need to be adjusted from editor.
    [HideInInspector] public bool cameraMoving;
    [HideInInspector] public bool cameraResize;

    private void Awake()
    {
        if (CMInstance == null)
            CMInstance = this;
        else if (CMInstance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Update ()
    {
        // This is an example of using the camera script, pressing the buttons you can move the camera.
        // After changing the script, you now only need to call one function to move the position and size of camera.

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    // To move the camera we call the MoveCamera function,
        //    // this handles the position and size of the camera.
        //    MoveCamera(playerPosition, playerSize);
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
        //currentPosition = cameraObject.transform.position;
        //currentSize = cameraObject.orthographicSize;

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