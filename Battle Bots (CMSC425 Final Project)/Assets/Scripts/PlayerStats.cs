using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public float playerHealth = 15f;
    public float playerDamage = 1f;
    public float playerSpeed = 1f;

	// Use this for initialization
	void Start () {
        PlayerPrefs.SetFloat("PlayerHealth", playerHealth);
        PlayerPrefs.SetFloat("PlayerHealth", playerHealth);
        PlayerPrefs.SetFloat("PlayerHealth", playerHealth);
    }

    public void TakeDamage(float amount)
    {

        playerHealth -= amount;
        Debug.Log(playerHealth);
        if (playerHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Menu");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
