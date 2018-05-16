using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedBotScript : MonoBehaviour {
	public float range = 50f;
	public float fireRate = 1f;
	private float nextTimeToFire = 0f;
	public GameObject impactEffect;

    public int damage;
    public int speed;
    public float accuracy;
    public int level;

    public GameObject player;
    private LayerMask ignoreself;

    public bool ally;
    private string side;
    private string oppositeside;

    private int layerMask;

    private float attackCooldown = 0f;

	Target self;
	NavMeshAgent agent;
	Animator animator;
	List<GameObject> enemies;
   
	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        self = GetComponent<Target>();

        agent.stoppingDistance = 8;

        if (ally)
        {
            layerMask = 1 << 9;
            layerMask = ~layerMask;
            side = "Friendly";
            oppositeside = "Enemy";
            level = PlayerPrefs.GetInt("ShooterLevel");
        }
        else
        {
            layerMask = 1 << 8;
            layerMask = ~layerMask;
            side = "Enemy";
            oppositeside = "Friendly";
            level = PlayerPrefs.GetInt("Level");
        }

        self.health = 10 + 5 * level;
        self.maxhealth = self.health;
        damage = 1 + 1 * level;
        speed = 1 + 1 * level;
        accuracy = .50f + .05f * level;
        agent.speed = speed;

        enemies = new System.Collections.Generic.List<GameObject>();
		populateEnemies ();
	}
	
	// Update is called once per frame
	void Update () {
		if (self.health > 0f)
		{
			GameObject currentEnemy;
            if (enemies.Count == 0)
            {
                animator.SetBool("Attack", false);
                populateEnemies();
            }
            if (enemies.Count > 0) {
                // Gets the closest enemy
                currentEnemy = enemies[0];
				if (currentEnemy != null && enemies.Count > 0) {
					float distanceToEnemy = Vector3.Distance (currentEnemy.transform.position, transform.position);
					if (distanceToEnemy <= range) {

                        if (agent != null)
                        {
                            agent.speed = 0;
                            agent.angularSpeed = 0;
                        }

                        FaceTarget (currentEnemy);

                        RaycastHit hit;
						Target enemy = currentEnemy.transform.GetComponent<Target> ();
						if (enemy.health <= 0 && Physics.Raycast(transform.position + new Vector3(0, .5f, -1f), (enemy.transform.position - transform.position).normalized, out hit, 100f, layerMask) 
                            && hit.transform.name.CompareTo(currentEnemy.transform.name) != 0) {
							enemies.Remove(currentEnemy);
                            populateEnemies();
                            if (enemies.Count > 0)
                            {
                                currentEnemy = enemies[0];
                            }
							animator.SetBool ("Attack", false);
						} else if (attackCooldown <= 0f) {
                            Debug.Log("Attacking");
							Shoot (currentEnemy, enemy);
                            attackCooldown = fireRate;
                        }
					}
                    else
                    {
                        populateEnemies();
                    }
				}
			}
			else if (ally)
			{
                agent.speed = 3.5f;
                agent.angularSpeed = 120f;
                animator.SetBool("Attack", false);
                agent.SetDestination(player.transform.position);
			}
		}
		attackCooldown -= Time.deltaTime;
	}

    // Movies to face the enemy
	void FaceTarget(GameObject enemy)
	{
		Vector3 direction = (enemy.transform.position - transform.position).normalized;
		Quaternion lookRoation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRoation, Time.deltaTime * 5f);
	}

    // Deals damage to the enemy shot at
	void AttackTarget(GameObject enemy, Target target)
	{
		if (target.health > 0) {
			target.TakeDamage (damage);
		} else {
            enemies.Remove(enemy);
		}
	}

    // Helper method to help visualize the range of the enemy
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
    }

    // Fires at the enemy. If hit, the enemy is attacked
	void Shoot(GameObject enemy, Target target){
		float success = Random.Range (0, 10f);
		Rigidbody rb = enemy.GetComponent<Rigidbody> ();

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, .5f, 0), (enemy.transform.position - transform.position).normalized, out hit, 100f, layerMask))
        {
            Debug.Log("Hit: " + hit.transform.name);
            if (target != null && hit.transform.tag == oppositeside)
            {
                animator.SetBool("Attack", true);
                if (success >= accuracy * 10)
                {

                    AttackTarget(enemy, target);

                    if (rb != null)
                    {

                        rb.AddForce(enemy.transform.position * 30);

                    }
                    if (impactEffect != null)
                    {
                        GameObject impact = Instantiate(impactEffect, new Vector3(enemy.transform.position.x + Random.Range(.5f, 1f), enemy.transform.position.y + Random.Range(0, 2.5f), enemy.transform.position.z - .5f), Quaternion.LookRotation(enemy.transform.position.normalized));
                        Destroy(impact, .1f);
                    }
                }
            }
            else
            {
                animator.SetBool("Attack", false);
            }
        }
	}

    // This function gets an array of all the enemies in range
	void populateEnemies() {
        GameObject[] tagged;
        if (side.CompareTo("Friendly") == 0)
        {
            tagged = GameObject.FindGameObjectsWithTag("Enemy");
        }
        else
        {
            tagged = GameObject.FindGameObjectsWithTag("Friendly");
        }
        if (tagged != null)
        {

            //enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

            float distToEnemy;
            RaycastHit hit;
            foreach (GameObject obj in tagged)
            {
                distToEnemy = Vector3.Distance(obj.transform.position, transform.position);
                if (distToEnemy <= range && obj.transform.GetComponent<Target>().health > 0
                    && Physics.Raycast(transform.position + new Vector3(0, .5f, 0), (obj.transform.position - transform.position).normalized, out hit, 100f, layerMask)
                    && hit.transform.tag == oppositeside)
                {
                    enemies.Add(obj);
                }
                else
                {
                    enemies.Remove(obj);
                }
            }
            enemies.Sort(SortByDistance);
        }
	}

    int SortByDistance(GameObject p1, GameObject p2)
    {
        float distToP1 = Vector3.Distance(p1.transform.position, transform.position);
        float distToP2 = Vector3.Distance(p2.transform.position, transform.position);
        return distToP1.CompareTo(distToP2);
    }

}
