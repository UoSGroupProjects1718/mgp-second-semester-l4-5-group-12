using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * RoundManager Script
 * 
 * - THERE WILL BE A DESCRIPTION HERE ONE DAY - 
 * 
 */

public class RoundManager : MonoBehaviour {

    public static RoundManager RMInstance;

    private void Awake()
    {
        if (RMInstance == null)
            RMInstance = this;
        else if (RMInstance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void StartGame ()
    {

    }

    public void ChangeRound  ()
    {

    }

    public void EndGame ()
    {

    }
}
