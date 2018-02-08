using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * GameManger Script
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

    public static GameManager Instance;

    // Most of the variables set below are just placeholders,
    // they are there to lay foundation of the script to later build upon.
    // Some of them will be removed, and more might be added.

    [Header("Scene Settings")]
    public Camera mapCamera;
    public GameObject playerOnePrefab;
    public GameObject playerTwoPrefab;

    [Header("Runtime Settings")]
    [Tooltip("This is the force that will be applied to the projectile, depending in which direction it will be facing.")]
    [SerializeField] private float windStrenght;


    private void Awake()
    {
        if (Instance == this)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
