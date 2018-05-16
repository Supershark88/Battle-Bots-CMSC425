
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Target : MonoBehaviour {

	public int health = 10;
    public bool player;

    private int playerDamageIncrease;
    private int playerSpeedIncrease;

    public int maxhealth; 

    private Animator animator;
    private BoxCollider box;
    private Rigidbody body;
    private NavMeshAgent nav;

	bool dead = false;

	void Start()
    {
        if (player)
        {
            health = PlayerPrefs.GetInt("PlayerHealth");
            playerDamageIncrease = PlayerPrefs.GetInt("PlayerDamage");
            playerSpeedIncrease = PlayerPrefs.GetInt("PlayerSpeed");
        }
        else
        {
            box = GetComponent<BoxCollider>();
            body = GetComponent<Rigidbody>();
            if (this.tag != "Inanimate")
            {
                animator = GetComponent<Animator>();
                nav = GetComponent<NavMeshAgent>();
            }
        }
        maxhealth = health;
	}

	public void TakeDamage(int amount)
    {
		
		health -= amount;

        if (animator != null)
        {

        }

		if (health <= 0f) {

			Die ();
		}
	}

    public void Heal(int amount)
    {

        health += amount;

        if (health > maxhealth)
        {
            health = maxhealth;
        }
    }

    public int maxHealth()
    {
        return maxhealth;
    }

    public int playerDamage()
    {
        return playerDamageIncrease;
    }

    public int playerSpeed()
    {
        return playerSpeedIncrease;
    }

    public bool isDead()
    {
        return dead;
    }

	private void Die() {
        if (player)
        {
            dead = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            if (gameObject.tag.CompareTo("Enemy") == 0)
            {
                int scrap = PlayerPrefs.GetInt("Scrap");
                PlayerPrefs.SetInt("Scrap", scrap + 100);
            }
            dead = true;
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            if (nav != null)
            {
                nav.speed = 0;
                nav.angularSpeed = 0;
            }
            animator.SetBool("dead", dead);
            //animator.enabled = true;
            box.enabled = false;
            Destroy(gameObject, 5f);
        }
	}
}
