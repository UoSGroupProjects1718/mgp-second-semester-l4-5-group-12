using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    [SerializeField] private int alphaScene;

    public void PlayGame()
    {
        SceneManager.LoadScene("alphaScene");
		Debug.Log("Playing the game.");
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene("tutorialScreen");
        Debug.Log("Tutorial screen loading.");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("You can't quit the game in editor. But at this point the application would close.");
    }
}
