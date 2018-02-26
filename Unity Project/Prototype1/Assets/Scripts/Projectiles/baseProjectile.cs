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

	[Header("Projectile Variables")]
    [Tooltip("This is how much damage the projectile will deal.")]
    [SerializeField] private float projectileDamage;


    void Update()
    {
        if (this.transform.position.y <= -10)
            Destroy(this.gameObject);
    }
}
