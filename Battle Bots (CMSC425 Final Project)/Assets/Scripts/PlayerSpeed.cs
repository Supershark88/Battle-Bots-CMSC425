using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerSpeed : MonoBehaviour {

    public int speed;

    // Use this for initialization
    void Start () {
		speed = PlayerPrefs.GetInt("PlayerSpeed");
        gameObject.transform.GetComponent<FirstPersonController>().setSpeed(speed); 
    }
}
