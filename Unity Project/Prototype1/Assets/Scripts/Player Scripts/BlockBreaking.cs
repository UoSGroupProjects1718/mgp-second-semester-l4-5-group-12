using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreaking : MonoBehaviour {

    [SerializeField] private string TagName;
    //Editable 'tag' attached to the block.
    private float PercentageChance = 0.25f;
    //This is the variable that determines if the block is broken or not in a collision.
    private float BreakChance;
    //Compared with PercentageChance after being randomly generated with every collision.
    

    void OnCollisionEnter2D(Collision2D collider)
    {     
        //When the projectile hits a block,
        if (collider.gameObject.tag == TagName)
        //If the projectile has the same tag as the block's 'tag',
        {
            BreakChance = Random.Range(0f, 1f);
            Debug.Log(BreakChance.ToString());
            //A random number between 0 and 1 is generated.
            if (BreakChance <= PercentageChance)
            //If this number is less than or equal to 0.25,
            {
                Destroy(this.gameObject);
                //Destroy the block (not the projectile).
            }
        }
    }
}