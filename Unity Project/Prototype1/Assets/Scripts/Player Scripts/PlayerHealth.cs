using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public int currentHealth;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int ProjectileDamage = 10;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        currentHealth -= ProjectileDamage;
        Destroy(collider.gameObject);
        // This is where the attacking player's turn would end, 
        // assuming that we only shoot one projectile per turn.
        // Power-ups could increase the number of projectiles,
        // so maybe change the location of this green text wall.
    }

}

