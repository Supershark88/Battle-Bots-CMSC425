using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    Image healthBar;

    public GameObject target;

    private int health;
    private int maxhealth;

	// Use this for initialization
	void Start () {
        healthBar = GetComponent<Image>();
        if (target != null)
        {
            health = target.transform.GetComponent<Target>().health;
            maxhealth = health;
        }
        else
        {
            health = 0;
            maxhealth = 1;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            health = target.transform.GetComponent<Target>().health;
            healthBar.fillAmount = (float)health / (float)maxhealth;
            if (target.transform.GetComponent<Target>().isDead())
            {
                target = null;
            }
        }
	}
}
