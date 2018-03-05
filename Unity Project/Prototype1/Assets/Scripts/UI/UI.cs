using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

    public void OnPlayButtonClick(string test01)
    {
        SceneManager.LoadScene (test01);
    }

    //public enum Scenes        //Probably delete this since it will be used in the level select.
    //{
    //    menuScreenTest,
    //    test01
    //}

    //public void OnQuitClick()         // Ignore this bit for now.
    //{
    //    Application.Quit();
    //}
}
