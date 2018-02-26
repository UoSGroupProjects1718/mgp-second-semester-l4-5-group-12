using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene((int)Scenes.menuScreenTest);
    }

    public enum Scenes
    {
        menuScreenTest,
        test01
    }

    //public void OnQuitClick()         // Ignore this bit for now.
    //{
    //    Application.Quit();
    //}
}
