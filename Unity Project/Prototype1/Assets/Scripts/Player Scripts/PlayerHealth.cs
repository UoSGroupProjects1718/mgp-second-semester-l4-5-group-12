using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public float currentHealth;

    public Text playerName;

    [SerializeField] private float maxHealth = 100;
    //Sets the maximum health of the player's core.
    [SerializeField] private float ProjectileDamage;
    //Sets the amount of damage that a projectile does to the core.
    [SerializeField] private string tagName;
    //Editable tag used to determine when damage is dealt.

    private void Start()
    {
        currentHealth = maxHealth;
        ProjectileDamage = GameManager.GMInstance.baseDamage;
        //Resets the current health to the maximum.
        playerName.text = gameObject.name;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            GameManager.GMInstance.currentRoundState = RoundState.GAMEOVER;
            GameManager.GMInstance.GameOverScreen();
            GameManager.GMInstance.winningPlayer.text = playerName + "Wins!";
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == tagName)
        {
            currentHealth -= ProjectileDamage * GameManager.GMInstance.currentTimeLimit;
            //Decrease the current health by the projectile damage.
            Destroy(collider.gameObject);
            //Destroy the projectile.

            // This is where the attacking player's turn would end, 
            // assuming that we only shoot one projectile per turn.
            // Power-ups could increase the number of projectiles,
            // so maybe change the location of this green text wall.

            
        }
    }

}

