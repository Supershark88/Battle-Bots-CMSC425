using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter (Collider other) {
        if (other.gameObject.CompareTag("Scrap"))
        {
            int scrap = PlayerPrefs.GetInt("Scrap");
            PlayerPrefs.SetInt("Scrap", scrap + 100);
            other.gameObject.SetActive(false);
        }
	}
}
