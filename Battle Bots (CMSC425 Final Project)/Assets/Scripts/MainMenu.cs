using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void PlayGame ()
    {
        PlayerPrefs.SetInt("Scrap", 500);
        SceneManager.LoadScene("Upgrades");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
