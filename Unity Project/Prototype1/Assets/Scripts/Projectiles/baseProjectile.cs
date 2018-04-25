using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Base Projectile Script
 * 
 * This is the base projectile script, we use this as a framework for our projectiles; this will have all the basic variables.
 * We then create our projectiles that will inherit from this script, and we assign variables on them.
 * 
 * Anything we create here will happen in the object that inherits from this script. For example, below I declare in Update that
 * when we are below '-10' on 'Y' axis we destroy the game object. This will happen on any object that inherits from this script.
 * 
 * Unless we override the function the behaviour will stay the same as it is.
 * 
 * This way we can have multiple projectiles, and we don't have to decleare the variables for each of them; we only create them once,
 * and after that we assign the values and we add any functionality if required.
 * 
 */

public class baseProjectile : MonoBehaviour {
    private float projectileDamage;
    private float speed;

    [HideInInspector] public int playerOwner;
    private int hitAmnt = 3;

    private void Start()
    {
        projectileDamage = GameManager.GMInstance.baseDamage;
    }

    private void Update()
    {
        if (this.transform.position.y <= -45)
            Destroy(this.gameObject);

        if (hitAmnt <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && playerOwner != other.gameObject.GetComponent<PlayerController>().playerNumber)
        {
            PlayerController _playerController = other.gameObject.GetComponent<PlayerController>();

            _playerController.currentHealth -= projectileDamage * GameManager.GMInstance.currentTimeLimit;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Block"))
        {
            baseBlock otherBB = other.gameObject.GetComponent<baseBlock>();

            otherBB.currentHealth -= 1;
            otherBB.UpdateHealth();

            hitAmnt -= 1;
        }

        if (other.gameObject.tag == "Player") 
        {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            Destroy(gameObject);
        }
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
