using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * GameManager Script
 * 
 * This is the GameManager script, it is used to control the current state of the game.
 * Ideally this also controls the players, but does not local variables for the players,
 * so it will provide a link between the players but things like health will be stored locally.
 * 
 * Once the game idea expands, this script will be used to handle more and more things,
 * most likely it will have its own functions and such; ideally have it work with a player manager,
 * so this way we can keep this script neater. For now, most things will be done here for the prototypes.
 * 
*/

public class GameManager : MonoBehaviour
{

    public static GameManager GMInstance;

    public enum RoundState { MENU, PLAYERONE, PLAYERTWO, INTERMISSION }

    // Most of the variables set below are just placeholders,
    // they are there to lay foundation of the script to later build upon.
    // Some of them will be removed, and more might be added.

    [Header("Scene Settings")]
    public Camera mapCamera;
    public GameObject playerOnePrefab;
    public GameObject playerTwoPrefab;

    [Header("Camera Settings")]
    public Vector3 playerOneCameraPos = new Vector3(-25, 0, -10);
    public Vector3 playerTwoCameraPos = new Vector3(25, 0, -10);
    public Vector3 intermissionPos = new Vector3(0, 0, -10);
    public float playerOneCameraSize = 10;
    public float playerTwoCameraSize = 10;
    public float intermissionCameraSize = 15;

    [Header("Turn Settings")]
    public RoundState currentRoundState = RoundState.INTERMISSION;
    public float turnDelay;

    private float currentTurnDelay;
    private bool intermission;

    void Awake ()
    {
        if (GMInstance == null)
            GMInstance = this;
        else if (GMInstance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        ChangeTurn();
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            ChangeTurn();
        }
    }

    void ChangeTurn() 
    {
        StartCoroutine(ChangeTurnIE());
    }

    IEnumerator ChangeTurnIE()
    {

        if (currentRoundState == RoundState.PLAYERONE)
        {
            // Move to player 2.
            CameraManager.CMInstance.MoveCamera(intermissionPos, intermissionCameraSize);
            currentRoundState = RoundState.INTERMISSION;

            yield return new WaitForSeconds(5f);

            CameraManager.CMInstance.MoveCamera(playerTwoCameraPos, playerTwoCameraSize);
            currentRoundState = RoundState.PLAYERTWO;

            StopCoroutine(ChangeTurnIE());
        } 
        else if (currentRoundState == RoundState.PLAYERTWO)
        {
            // Move to player 1.
            CameraManager.CMInstance.MoveCamera(intermissionPos, intermissionCameraSize);
            currentRoundState = RoundState.INTERMISSION;

            yield return new WaitForSeconds(5f);

            CameraManager.CMInstance.MoveCamera(playerOneCameraPos, playerOneCameraSize);
            currentRoundState = RoundState.PLAYERONE;

            StopCoroutine(ChangeTurnIE());
        }
        else if (currentRoundState == RoundState.INTERMISSION)
        {
            // Move to player 1
            currentRoundState = RoundState.INTERMISSION;
            CameraManager.CMInstance.MoveCamera(intermissionPos, intermissionCameraSize);

            yield return new WaitForSeconds(5f);

            currentRoundState = RoundState.PLAYERONE;
            CameraManager.CMInstance.MoveCamera(playerOneCameraPos, playerOneCameraSize);

            StopCoroutine(ChangeTurnIE());
        }

        StopCoroutine(ChangeTurnIE());
    }
}
