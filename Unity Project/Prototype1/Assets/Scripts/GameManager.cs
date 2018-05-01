using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

public enum RoundState { MENU, INTERMISSION, PLAYING, GAMEOVER }

public class GameManager : MonoBehaviour
{

    public static GameManager GMInstance;

    //public enum RoundState { MENU, INTERMISSION, PLAYING, GAMEOVER }

    [Header("Player's Settings")]
    [SerializeField] private Vector3 playerOneCameraPos; //= new Vector3(-25, 0, -10);
    [SerializeField] private Vector3 playerTwoCameraPos; //= new Vector3(25, 0, -10);
    [SerializeField] private Vector3 intermissionPos; //= new Vector3(0, 0, -10);
    [SerializeField] private float playerOneCamSize = 10;
    [SerializeField] private float playerTwoCamSize = 10;
    [SerializeField] private float intermissionCamSize = 15;
    public float baseDamage = 5;

    [Header("Turn Settings")]
    public RoundState currentRoundState = RoundState.INTERMISSION; 
    [SerializeField] private Text turnText; 
    [SerializeField] private Text timerText; 
    [SerializeField] private float intervalTime;
    [SerializeField] private int turnCounter;

    [SerializeField] public GameObject restartButton;
    [SerializeField] public Text winningPlayer;
    [SerializeField] public bool isGameOver = false;
    public GameObject gameOverText;

    [Header("Debug Stuff")]
    public int playerTurn; 
    [SerializeField] private float currentIntervalTime;

    [Header("Countdown Stuff")]
    public float currentTimeLimit;
    [SerializeField] private float timeLimit;

    [HideInInspector] public bool canShoot; 
    [HideInInspector] public bool cameraMoving; 

    private Camera mapCamera;
    private GameObject playerOne;
    private GameObject playerTwo;

    [HideInInspector] public GameObject shootingAim;
    private GameObject[] players;

    //private bool currentTimeLimit;
    private bool isCountingDown = false; 
    private bool intermissionCounter;
    public bool isProjectile = false;

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
        isGameOver = false;
        isCountingDown = false;
        isProjectile = false;

        AssignPlayers();

        if (!mapCamera)
        {
            mapCamera = Camera.main;
        }
        else
        {
            Debug.Log("Camera already referenced? HOW?");
        }

        gameOverText.SetActive(false);
        restartButton.SetActive(false);
        currentIntervalTime = intervalTime;
        Physics2D.IgnoreLayerCollision(0, 9, true);
        Physics2D.IgnoreLayerCollision(8, 9, true);

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
        // The camera is not moving.
        if (!cameraMoving && turnText.isActiveAndEnabled && !isProjectile) 
        {
            timerText.enabled = false; 
            turnText.enabled = false;

            canShoot = true;
        }

        if (!cameraMoving && currentRoundState != RoundState.INTERMISSION)
        {
            isCountingDown = true;
        }

        // Count down if its intermission.
        if (intermissionCounter == true)
        {
            currentIntervalTime -= Time.deltaTime;
            timerText.text = currentIntervalTime.ToString("0");

            if (currentIntervalTime <= 0)
            {
                intermissionCounter = false;
                currentIntervalTime = intervalTime;
                currentRoundState = RoundState.PLAYING;
                HandleTurnChange();
            }
        }

        // Round time counter.
        if (isCountingDown == true)
        {
            currentTimeLimit -= Time.deltaTime;
            timerText.text = currentTimeLimit.ToString("0");

            if (currentTimeLimit <= 0)
            {
                isCountingDown = false;
                currentTimeLimit = timeLimit;
                timerText.enabled = false;
                              
                ChangeTurn();
            }
        }
    }

    public void ChangeTurn ()
    {
        Destroy(shootingAim);

        currentRoundState = RoundState.INTERMISSION;

        CameraManager.CMInstance.MoveCamera(intermissionPos, intermissionCamSize);

        turnText.text = "Intermission";

        // Change turn currentTimeLimit.
        currentIntervalTime = intervalTime;
        currentTimeLimit = timeLimit;

        intermissionCounter = true;
        turnText.enabled = true;
        timerText.enabled = true;

        canShoot = false;
        isCountingDown = false;
    }

    //TURN CHANGING
    private void HandleTurnChange () 
    {
        timerText.enabled = false;

        if (playerTurn == 0)
        {
            // The game just began, do stuff and start the game.
            CameraManager.CMInstance.MoveCamera(playerOneCameraPos, playerOneCamSize);

            playerTurn += 1;
            turnText.text = "Player 1 Turn.";
        }
        else if (playerTurn == 1)
        {
            // It's player 1's turn -> Change to player 2.
            CameraManager.CMInstance.MoveCamera(playerTwoCameraPos, playerTwoCamSize);

            playerTurn += 1;
            turnText.text = "Player 2 Turn.";
        }
        else if (playerTurn == 2)
        {
            // It's player 2's turn -> Change to player 1.
            CameraManager.CMInstance.MoveCamera(playerOneCameraPos, playerOneCamSize);

            playerTurn += 1;
            turnText.text = "Player 1 Turn.";
        }

        // If the turn number is higher than the player's we have, we go back to player 1.
        if (playerTurn > players.Length)
        {
            playerTurn = 1;
        }

        turnCounter += 1;
        currentTimeLimit = timeLimit;
    }

    public void GameOverScreen()
    {
        if (isGameOver == true)
        {
            Debug.Log("GameOver");
            isCountingDown = false;
            gameOverText.SetActive(true);
            restartButton.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Debug.Log("Game restarted");
        SceneManager.LoadScene("menuScreen");
        gameOverText.SetActive(false);
        restartButton.SetActive(false);
        isGameOver = false;
    }
}
