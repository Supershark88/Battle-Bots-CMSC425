using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour
{
    private TextMeshProUGUI healthText;

    public GameObject target;

    private int health;
    private int maxhealth;

    // Use this for initialization
    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
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
    void Update()
    {
        if (target != null)
        {
            health = target.transform.GetComponent<Target>().health;
            if (health < 0)
            {
                health = 0;
            }
            healthText.text = "Health: " + health;
            if (target.transform.GetComponent<Target>().isDead())
            {
                target = null;
            }
        }
    }
}
