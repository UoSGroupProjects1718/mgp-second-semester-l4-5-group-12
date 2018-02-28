using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public int playerNumber = 0;

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
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
