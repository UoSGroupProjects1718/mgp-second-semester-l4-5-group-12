using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    [SerializeField] private int alphaScene;

    public void PlayGame()
    {
        SceneManager.LoadScene("alphaScene");
        GameManager.GMInstance.isGameOver = false;
        GameManager.GMInstance.gameOverText.SetActive(false);
        GameManager.GMInstance.restartButton.SetActive(false);
        gameObject.GetComponent<CameraManager>().enabled = true;
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
