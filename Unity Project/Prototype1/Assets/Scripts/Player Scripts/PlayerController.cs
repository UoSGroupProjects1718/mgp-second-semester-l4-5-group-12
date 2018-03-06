using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * PlayerController Script
 * 
 * This is the player controller script, this is where all player related stuff is handled.
 * At the moment all the script does, is it has a variable to see if the player is P1 or P2.
 * It keep track of player's max health and current health amount.
 * 
 */

public class PlayerController : MonoBehaviour {

    [Header("Player Settings")]
    [Tooltip("This is the player index (player number). Setting this to 1 will make this object Player 1, setting this to 2 will make this Player 2, etc. Make sure you don't have multiple players with the same number.")]
    public int playerNumber = 0;
    [Tooltip("This is the health the player has at the beginning of the round; this will most likely be changed later when we develop the game further.")]
    [SerializeField] private float playerHealth;
    public Text healthText;

    [HideInInspector] public float currentHealth;

	void Start ()
    {
        if (playerNumber > 2 || playerNumber < 1)
        {
            Debug.LogError("Player out of range. We only have two players in this game.... Duh! (" + gameObject.name + ")");
        }
        else if (playerNumber == 0)
        {
            Debug.LogError("You have not assigned the player number at: " + gameObject.name);
        }

        currentHealth = playerHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (healthText)
        {
            if (currentHealth >= 10)
                healthText.text = currentHealth.ToString("00");
            else
                healthText.text = currentHealth.ToString("0");
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player det: " + gameObject.name);
        }
    }
}
