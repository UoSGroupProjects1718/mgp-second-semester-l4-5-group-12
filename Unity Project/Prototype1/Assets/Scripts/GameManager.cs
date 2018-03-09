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
 *  - The camera moves to where the player's base is (zoomed out),
 *      but once the player taps to start the turn the camera zooms in.
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
    [SerializeField] private Text turnText; //Text showing the turn
    [SerializeField] private Text timerText; //Text showing the remaining time in that turn
    [SerializeField] private float turnDelay; //Time between each turn (interval time)
    [SerializeField] private int turnCounter; //Keeps track of whose turn it is

    // Debug duh.
    [Header("Debug Stuff")]
    public int playerTurn;  //Why is this a public variable?
    [SerializeField] private float currentTurnDelay; //Counts down the interval time, is reset by setting as turnDelay

    [HideInInspector] public bool canShoot; //Is shooting enabled?
    [HideInInspector] public bool cameraMoving; //Is the camera moving?

    private Camera mapCamera;
    private GameObject playerOne;
    private GameObject playerTwo;

    private GameObject[] players;

    //private bool currentTimeLimit;
    [Header("Countdown Stuff")]
    public float currentTimeLimit;  //Time remaining in the current turn, is reset by setting as timeLimit 
    [SerializeField] private float timeLimit; //Time for each turn
    private bool isCountingDown = false; //Is the countdown currently running?
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
                    //playerOneCameraPos = _go.transform.position;
                    //playerOneCameraPos.z = -10;
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
                    //playerTwoCameraPos = _go.transform.position;
                    //playerTwoCameraPos.z = -10;
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
        if (!cameraMoving && turnText.isActiveAndEnabled)
        {
            timerText.enabled = false;
            turnText.enabled = false;
            canShoot = true;
        }

        // Our timer for turn delay.
        if (intermissionCounter == true)
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
            timerText.text = currentTurnDelay.ToString("0");

        if(isCountingDown == true) //if the timer is counting down
        {
            if (currentTimeLimit <= 0)
            {
                isCountingDown = false;
                currentTimeLimit = timeLimit;
                currentRoundState = RoundState.INTERMISSION;
                HandleTurnChange();
            }
        }

            currentTimeLimit -= Time.deltaTime;
            timerText.text = currentTimeLimit.ToString("0");
                   
        }
    }

    public void ChangeTurn ()
    {
        currentRoundState = RoundState.INTERMISSION;

        CameraManager.CMInstance.MoveCamera(intermissionPos, intermissionCamSize);

        turnText.text = "Intermision";   

        // Change turn currentTimeLimit.
        currentTurnDelay = turnDelay;
        canShoot = false;
        intermissionCounter = true;
        turnText.enabled = true;
        timerText.enabled = true;
        currentTimeLimit = timeLimit;
    }

    private void UpdateUI() 
    {

    }

    private void HandleTurnChange () 
    {
        //StartCoroutine(ChangeTurnIE());
        //Debug.Log("Changing turn.");
        timerText.enabled = false;

        if (playerTurn == 0)
        {
            // Debug.Log("Changing to player 1");
            // The game just began, do stuff and start the game.

            CameraManager.CMInstance.MoveCamera(playerOneCameraPos, playerOneCamSize);

            playerTurn += 1;
            //canShoot = true;
            turnText.text = "Player 1 Turn.";
            isCountingDown = true;
        }
        else if (playerTurn == 1)
        {
            // Debug.Log("Changing to player 2");
            // It's player 1's turn -> Change to player 2.

            CameraManager.CMInstance.MoveCamera(playerTwoCameraPos, playerTwoCamSize);

            playerTurn += 1;
            //canShoot = true;
            turnText.text = "Player 2 Turn.";
            isCountingDown = true;
        }
        else if (playerTurn == 2)
        {
            // Debug.Log("Changing to player 1");
            // It's player 2's turn -> Change to player 1.

            CameraManager.CMInstance.MoveCamera(playerOneCameraPos, playerOneCamSize);

            playerTurn += 1;
            //canShoot = true;
            turnText.text = "Player 1 Turn.";
            isCountingDown = true;
        }

        // If the turn number is higher than the player's we have, we go back to player 1.
        if (playerTurn > players.Length) {
            playerTurn = 1;
            isCountingDown = true;
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
