using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreaking : MonoBehaviour {

    [SerializeField] private string TagName;  
    //Editable 'tag' attached to the block.
    [SerializeField] private float BreakChance;
    //Used to determine if the block is broken or not in a collision.

    void OnCollisionEnter2D(Collision2D collider) {     
        //When the projectile hits a block,
        if (collider.gameObject.tag == TagName)
        //If the projectile has the same tag as the block's 'tag',
        {
            BreakChance = Random.Range(0f, 1f);
            Debug.Log(BreakChance.ToString());
            //A random number between 0 and 1 is generated.
            if (BreakChance <= 0.25)
            //If this number is less than or equal to 0.25,
            {
                Destroy(this.gameObject);
                //Destroy the block (not the projectile).
            }
        }
    }
}