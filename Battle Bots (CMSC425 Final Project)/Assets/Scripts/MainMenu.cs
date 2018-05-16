using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame ()
    {
        PlayerPrefs.SetInt("Scrap", 500);
        PlayerPrefs.SetInt("PlayerHealth", 15);
        PlayerPrefs.SetInt("PlayerDamage", 1);
        PlayerPrefs.SetInt("PlayerSpeed", 3);

        PlayerPrefs.SetInt("ShooterLevel", 0);
        PlayerPrefs.SetInt("EngineerLevel", 0);
        PlayerPrefs.SetInt("BrawlerLevel", 0);
        PlayerPrefs.SetInt("Level", 0);

        SceneManager.LoadScene("Upgrades");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
