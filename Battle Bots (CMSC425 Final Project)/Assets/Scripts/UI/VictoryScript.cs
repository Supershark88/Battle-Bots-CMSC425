using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class VictoryScript : MonoBehaviour {

    private TextMeshProUGUI victoryText;
    private int enemies;

	// Use this for initialization
	void Start () {
        victoryText = gameObject.transform.GetComponent<TextMeshProUGUI>();
        GameObject[] tagged;
        tagged = GameObject.FindGameObjectsWithTag("Enemy");
        enemies = tagged.Length;
        victoryText.text = "";
    }
	
	// Update is called once per frame
	void Update () {
        GameObject[] tagged;
        tagged = GameObject.FindGameObjectsWithTag("Enemy");
        enemies = tagged.Length;
        if (enemies == 0)
        {
            victoryText.text = "Mission Accomplished" + System.Environment.NewLine + "Press E to return to Base";
            if (Input.GetKeyDown("e"))
            {
                int level = PlayerPrefs.GetInt("Level");
                PlayerPrefs.SetInt("Level", level + 1);
                SceneManager.LoadScene("Upgrades");
            }
        }
        else
        {
            victoryText.text = "";
        }
    }
}
