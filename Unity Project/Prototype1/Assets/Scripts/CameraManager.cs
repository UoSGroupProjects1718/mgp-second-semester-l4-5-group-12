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

    public enum RoundState
    {
        PlayerOne,
        PlayerTwo,
        Intermission
    }

    [Header("Camera Settings")]
    [Tooltip("Reference to the camera used in the scene, this will be the camera that will move when players change turns.")]
    public Camera cameraObject;
    [Tooltip("This is how long (in seconds) it will take the camera to move from position A to position B.")]
    public float cameraPanLength;
    [Tooltip("This is how long (in seconds) it will take the camera to scale from size A to size B.")]
    public float cameraZoomLength;

    // ANY DEBUG VARIABLES BELOW; HIDE WHEN DONE.. pls.
    [Header("Debug Stuff")]
    public Transform playerOnePosition;
    public Transform playerTwoPosition;
    public Transform intermissionPosition;
    public Vector3 cameraEndPosition;
    public float cameraEndSize;

    private Vector3 startPosition;
    private float cameraStartSize;
    private float timeStartedLerping;
    private bool isLerping;

    void Awake()
    {
        if (CMInstance == null)
            CMInstance = this;
        else if (CMInstance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start ()
    {
        startPosition = cameraObject.gameObject.transform.position;
        cameraStartSize = cameraObject.orthographicSize;

        StartLerping();
	}

    private void StartLerping()
    {
        isLerping = true;
        timeStartedLerping = Time.time;

        //startPosition = cameraObject.transform.position;
    }

    private void Update ()
    {
        // Wait for player input to start lerp.
        if (Input.GetKeyDown(KeyCode.Space))
            StartLerping();
    }

    private void FixedUpdate()
    {
        if (isLerping)
        {
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageComplete = timeSinceStarted / cameraPanLength;

            cameraObject.transform.position = Vector3.Lerp(startPosition, cameraEndPosition, percentageComplete);

            if (percentageComplete >= 1.0f)
                isLerping = false;
        }
    }

    public void SetNewCameraPosition(Vector3 newPosition)
    {

    }

    public void SetNewCameraSize(float newSize)
    {

    }
}
