using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityStandardAssets.Characters.FirstPerson;


public class DefeatScript : MonoBehaviour {

    public GameObject player;
    public CanvasGroup blackscreen;

    private TextMeshProUGUI defeatText;

    // Use this for initialization
    void Start () {
        defeatText = gameObject.transform.GetComponent<TextMeshProUGUI>();
        defeatText.text = "";
    }
	
	// Update is called once per frame
	void Update () {
		if (player.transform.GetComponent<Target>().isDead())
        {
            defeatText.text = "Mission Failed" + System.Environment.NewLine + "Press E to Restart";
            blackscreen.alpha = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            player.transform.GetComponent<FirstPersonController>().setSpeed(0);
            if (Input.GetKeyDown("e"))
            {
                SceneManager.LoadScene("Menu");
            }
        }
	}
}
