using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public int currentHealth;

    [SerializeField] private int maxHealth = 100;
    //Sets the maximum health of the player's core.
    [SerializeField] private int ProjectileDamage = 10;
    //Sets the amount of damage that a projectile does to the core.
    [SerializeField] private string tagName;
    //Editable tag used to determine when damage is dealt.

    private void Start()
    {
        currentHealth = maxHealth;
        //Resets the current health to the maximum.
    }

    void OnCollisionEnter2D(Collision2D collider)
        //When a projectile hits the core,
    {
        if (collider.gameObject.tag == tagName)
            //If the projectile's tag is the same as the core's 'tag',
        {
            currentHealth -= ProjectileDamage;
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

