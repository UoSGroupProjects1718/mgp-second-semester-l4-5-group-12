using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
 * At the moment we don't need a turn timer, it will only become annoying when we test the game.
 * 
 * TODO:     
 *  - Between the turns there should be "perk" drop (imagine WORMS game)
 *  
*/

public class GameManager : MonoBehaviour
{

    public static GameManager GMInstance;

    public enum RoundState { MENU, INTERMISSION, PLAYING }

    // Most below will be probably assigned in script some time soon.
    [Header("Player's Positions")]
    [SerializeField] private Vector3 playerOneCameraPos = new Vector3(-25, 0, -10);
    [SerializeField] private Vector3 playerTwoCameraPos = new Vector3(25, 0, -10);
    [SerializeField] private Vector3 intermissionPos = new Vector3(0, 0, -10);
    [SerializeField] private float playerOneCamSize = 10;
    [SerializeField] private float playerTwoCamSize = 10;
    [SerializeField] private float intermissionCamSize = 15;

    // Don't think this needs to be public?
    [Header("Turn Settings")]
    public RoundState currentRoundState = RoundState.INTERMISSION;
    [SerializeField] private float turnDelay;
    [SerializeField] private int turnCounter;

    // Debug duh.
    [Header("Debug Stuff")]
    public int playerTurn;
    [SerializeField] private float currentTurnDelay;

    [HideInInspector] public bool canShoot;
    [HideInInspector] public bool cameraMoving;

    private Camera mapCamera;
    private GameObject playerOne;
    private GameObject playerTwo;

    private GameObject[] players;

    private bool countDown;
    private bool intermissionCounter;

    private void Awake () 
    {
        if (GMInstance == null)
            GMInstance = this;
        else if (GMInstance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start () 
    {
        AssignPlayers();

        if (!mapCamera)
        {
            mapCamera = Camera.main;
        }
        else
        {
            Debug.Log("Camera already referenced? HOW?");
        }

        ChangeTurn();
	}

    private void AssignPlayers ()
    {
        // We find all the players in the scene, because we do this at the start it does not impact our performance.
        // But if we have done this often enough, we would see a performance drop.
        players = GameObject.FindGameObjectsWithTag("Player");

        // If there is no players found, throw an error.
        if (players.Length == 0)
        {
            Debug.LogError("NO PLAYERS FOUND.");
            return;
        }

        // Run a foreach loop and assign the players to the fields.
        foreach (GameObject _go in players)
        {
            int _playerNumber = _go.GetComponent<PlayerController>().playerNumber;

            if (_playerNumber == 0)
            {
                // The player number is not assigned (or left as default)
                Debug.LogError("No number assigned to player: " + _go.name);
            }
            else if (_playerNumber == 1)
            {
                // Before we assign player one, we make sure we haven't assigned anything here yet.
                if (!playerOne)
                {
                    playerOne = _go;
                    playerOneCameraPos = _go.transform.position;
                    playerOneCameraPos.z = -10;
                }
                else
                {
                    Debug.LogError("PLAYER 1 HAS ALREADY BEEN ASSIGNED.");
                }
            }
            else if (_playerNumber == 2)
            {
                // Before we assign player one, we make sure we haven't assigned anything here yet.
                if (!playerOne)
                {
                    playerTwo = _go;
                    playerTwoCameraPos = _go.transform.position;
                    playerTwoCameraPos.z = -10;
                }
                else
                {
                    Debug.LogError("PLAYER 2 HAS ALREADY BEEN ASSIGNED.");
                }
            }
            else
            {
                // Technically this shouldn't show up, because in player controller we force it to be either 1 or 2.
                Debug.LogError("Well... this shouldn't be appearing.");
            }
        }
    }

    private void Update ()
    {
        // Our timer for turn delay.
        if (intermissionCounter)
        {
            // Once we hit zero, we change the turn.
            if (currentTurnDelay <= 0)
            {
                intermissionCounter = false;
                currentTurnDelay = 0;
                currentRoundState = RoundState.PLAYING;
                HandleTurnChange();
            }

            // Take deltaTime every frame.
            currentTurnDelay -= Time.deltaTime;
        }
    }

    public void ChangeTurn ()
    {
        currentRoundState = RoundState.INTERMISSION;

        CameraManager.CMInstance.MoveCamera(intermissionPos, intermissionCamSize);

        

        // Change turn countdown.
        currentTurnDelay = turnDelay;
        canShoot = false;
        intermissionCounter = true;
    }

    private void HandleTurnChange () 
    {
        //StartCoroutine(ChangeTurnIE());
        //Debug.Log("Changing turn.");

        if (playerTurn == 0)
        {
            // Debug.Log("Changing to player 1");
            // The game just began, do stuff and start the game.

            CameraManager.CMInstance.MoveCamera(playerOneCameraPos, playerOneCamSize);

            playerTurn += 1;
            canShoot = true;
        }
        else if (playerTurn == 1)
        {
            // Debug.Log("Changing to player 2");
            // It's player 1's turn -> Change to player 2.

            CameraManager.CMInstance.MoveCamera(playerTwoCameraPos, playerTwoCamSize);

            playerTurn += 1;
            canShoot = true;
        }
        else if (playerTurn == 2)
        {
            // Debug.Log("Changing to player 1");
            // It's player 2's turn -> Change to player 1.

            CameraManager.CMInstance.MoveCamera(playerOneCameraPos, playerOneCamSize);

            playerTurn += 1;
            canShoot = true;
        }

        // If the turn number is higher than the player's we have, we go back to player 1.
        if (playerTurn > players.Length) {
            playerTurn = 1;
        }

        turnCounter += 1;
    }

    IEnumerator DropPerkCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        // .. do stuff here
    }

    public Coroutine DropPerk()
    {
        return StartCoroutine(DropPerkCoroutine());
    }
}
