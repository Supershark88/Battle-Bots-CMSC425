using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UpgradeManager : MonoBehaviour {

    private TextMeshProUGUI scrapText;
    private int scrap;

    private void Start()
    {
        scrap = PlayerPrefs.GetInt("Scrap");
        scrapText = gameObject.transform.Find("Scrap").GetComponent<TextMeshProUGUI>();
        scrapText.text = "Scrap: " + scrap;
    }

    public void AddPlayerHealth()
    {
        if (scrap >= 100)
        {
            scrap = scrap - 100;
            PlayerPrefs.SetInt("Scrap", scrap);
        }
        scrapText.text = "Scrap: " + scrap;
    }

    public void ContinueToGame()
    {
        SceneManager.LoadScene("Level Tester");
    }
}
