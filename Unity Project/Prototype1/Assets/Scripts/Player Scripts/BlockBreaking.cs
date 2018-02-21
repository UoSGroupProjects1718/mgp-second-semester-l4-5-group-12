using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreaking : MonoBehaviour {

    [SerializeField] private string tagName;
    [SerializeField] private float BreakChance;

    void OnCollisionEnter2D(Collision2D collider) {
        if (collider.gameObject.tag == tagName)
        {
            BreakChance = Random.Range(0, 1);
            if (BreakChance <= 0.25)
            {
                Destroy(this.gameObject);
            }
        }
    }
}