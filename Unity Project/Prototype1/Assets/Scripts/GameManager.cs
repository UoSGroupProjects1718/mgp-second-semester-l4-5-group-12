using System.Collections;
using System.Collections.Generic;
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
 * TODO:
 *  - Improve the turn change system, try to avoid using IEnumerator? (ask Chris about this)
 *      At the moment we don't need a turn timer, it will only become annoying when we test the game.
 *      
 *  - Between the turns there should be "perk" drop (imagine WORMS game)
 *  
*/

public class GameManager : MonoBehaviour
{

    public static GameManager GMInstance;

    public enum RoundState { MENU, PLAYERONE, PLAYERTWO, INTERMISSION }

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

    [Header("Debug Stuff")]
    [SerializeField] private int playerTurn;

    private Camera mapCamera;
    private GameObject playerOne;
    private GameObject playerTwo;


    private GameObject[] players;
    private float currentTurnDelay;

    private bool countDown;

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
        AssignPlayers();

        if (!mapCamera)
        {
            mapCamera = Camera.main;
        }
        else
        {
            Debug.Log("Camera already referenced? HOW?");
        }


        //ChangeTurn();
        //currentTurnDelay = turnDelay;
	}

    private void AssignPlayers()
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
                    playerOne = _go;
                else
                    Debug.LogError("PLAYER 1 HAS ALREADY BEEN ASSIGNED.");
            }
            else if (_playerNumber == 2)
            {
                // Before we assign player one, we make sure we haven't assigned anything here yet.
                if (!playerTwo)
                    playerTwo = _go;
                else
                    Debug.LogError("PLAYER 2 HAS ALREADY BEEN ASSIGNED.");
            }
            else
            {
                // Technically this shouldn't show up, because in player controller we force it to be either 1 or 2.
                Debug.LogError("Well... this shouldn't be appearing.");
            }
        }
    }

    void Update()
    {
        
    }

    void ChangeTurn() 
    {
        //StartCoroutine(ChangeTurnIE());


    }

    //IEnumerator ChangeTurnIE()
    //{
    //    if (currentRoundState == RoundState.PLAYERONE)
    //    {
    //        // Move to player 2.
    //        CameraManager.CMInstance.MoveCamera(intermissionPos, intermissionCameraSize);
    //        currentRoundState = RoundState.INTERMISSION;

    //        yield return new WaitForSeconds(CameraManager.CMInstance.cameraPanLength);
    //        StartRoundDelayTimer();
    //        yield return new WaitForSeconds(turnDelay);

    //        CameraManager.CMInstance.MoveCamera(playerTwoCameraPos, playerTwoCameraSize);
    //        currentRoundState = RoundState.PLAYERTWO;

    //        StopCoroutine(ChangeTurnIE());
    //    } 
    //    else if (currentRoundState == RoundState.PLAYERTWO)
    //    {
    //        // Move to player 1.
    //        CameraManager.CMInstance.MoveCamera(intermissionPos, intermissionCameraSize);
    //        currentRoundState = RoundState.INTERMISSION;

    //        yield return new WaitForSeconds(CameraManager.CMInstance.cameraPanLength);
    //        StartRoundDelayTimer();
    //        yield return new WaitForSeconds(turnDelay);

    //        CameraManager.CMInstance.MoveCamera(playerOneCameraPos, playerOneCameraSize);
    //        currentRoundState = RoundState.PLAYERONE;

    //        StopCoroutine(ChangeTurnIE());
    //    }
    //    else if (currentRoundState == RoundState.INTERMISSION)
    //    {
    //        // Move to player 1
    //        currentRoundState = RoundState.INTERMISSION;
    //        CameraManager.CMInstance.MoveCamera(intermissionPos, intermissionCameraSize);

    //        yield return new WaitForSeconds(CameraManager.CMInstance.cameraPanLength);
    //        StartRoundDelayTimer();
    //        yield return new WaitForSeconds(turnDelay);

    //        currentRoundState = RoundState.PLAYERONE;
    //        CameraManager.CMInstance.MoveCamera(playerOneCameraPos, playerOneCameraSize);

    //        StopCoroutine(ChangeTurnIE());
    //    }

    //    StopCoroutine(ChangeTurnIE());
    //}


   //[Header("Dan2's Countdown Settings")]     - Only here so I don't need to keep scrolling up and down.
   //public int DefaultCountdownTime = 15;       Delete when done.
   //public int RemainingTime = 15;
   //public Text TimeLeft;

   // void CountDown()
   // { 
   //     StartCoroutine(CountDownIE());     
   // }

   //IEnumerator CountDownIE() 
   //{
     //   yield return new WaitForSeconds(1); //Waits one second
      //  RemainingTime -= 1; //Decrements the remaining time
        //TimeLeft.text = RemainingTime.ToString(); //Converts the remaining time to a string
       // Debug.Log(TimeLeft.text); //Prints the remaining time to the console as a string
        //if (RemainingTime <= 0) //Checks if the time has run out
        //{
       //     ChangeTurn(); //Changes to the next intermission
       // }
        //CountDown and CountDownIE are incomplete and so not functional at the moment.
    //}
}
