using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class friendlyScript : MonoBehaviour {
	public float range = 50f;
	public float damage = .25f;
	public float fireRate = 1f;
	private float nextTimeToFire = 0f;
	public GameObject impactEffect;

	public GameObject targ;

    public string side = "Friendly";
    private string oppositeside;

    private float attackCooldown = 0f;
	public float hitSuccessThreshold = 5f;

	Target self;
	Transform target;
	NavMeshAgent agent;
	Animator animator;
	List<GameObject> enemies;
	PlayerStats player;
   
	// Use this for initialization
	void Start () {
		target = targ.transform;
		agent = GetComponent<NavMeshAgent>();
		enemies = new System.Collections.Generic.List<GameObject>();

		populateEnemies ();

		animator = GetComponent<Animator>();
		self = GetComponent<Target>();
		player = targ.GetComponent<PlayerStats>();

        if (side.CompareTo("Friendly") == 0)
        {
            oppositeside = "Enemy";
        }
        else
        {
            oppositeside = "Friendly";
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (self.health > 0f)
		{
			float distance = Vector3.Distance(target.position, transform.position);
			GameObject currentEnemy;
            if (enemies.Count == 0)
            {
                populateEnemies();
            }
            Debug.Log("Enemies: " + enemies.Count);
            if (enemies.Count > 0) {
                // Gets the first enemy seen
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

						Target enemy = currentEnemy.transform.GetComponent<Target> ();
						if (enemy.health <= 0) {
							enemies.Remove(currentEnemy);
                            populateEnemies();
                            if (enemies.Count > 0)
                            {
                                currentEnemy = enemies[0];
                            }
							animator.SetBool ("Attack", false);
						} else if (attackCooldown <= 0f) {
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
			else
			{
                animator.SetBool("Attack", false);
                agent.SetDestination(target.position);
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
            Debug.Log("Health: " + target.health);
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
        Debug.Log("Roll: " + success);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 2f, 0), (enemy.transform.position - transform.position).normalized, out hit, 100f))
        {
            Debug.Log(hit.transform.position);
            Debug.Log(hit.transform.name);
            if (target != null && hit.transform.tag == "Enemy")
            {
                animator.SetBool("Attack", true);
                if (success >= hitSuccessThreshold)
                {

                    AttackTarget(enemy, target);

                    if (rb != null)
                    {

                        rb.AddForce(enemy.transform.position * 30);

                    }
                    if (impactEffect != null)
                    {
                        GameObject impact = Instantiate(impactEffect, new Vector3(enemy.transform.position.x + Random.Range(.5f, 1f) + 1f, enemy.transform.position.y + Random.Range(0, 2.5f), enemy.transform.position.z), Quaternion.LookRotation(enemy.transform.position.normalized));
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
        tagged = GameObject.FindGameObjectsWithTag("Enemy");
        if (tagged != null) {
			
			//enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

			float distToEnemy;
			foreach (GameObject obj in tagged) {
				distToEnemy = Vector3.Distance (obj.transform.position, transform.position);
                Debug.Log("Enemies Name: " + obj.transform.name);
                if (distToEnemy <= range && obj.transform.GetComponent<Target>().health > 0) {
                    enemies.Add (obj);
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
