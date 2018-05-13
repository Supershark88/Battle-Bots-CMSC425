using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour {

    public float lookRadius = 10f;
    public float attackSpeed = 2f;
    public float attackDamage = 5f;

    public string side;

    private float attackCooldown = 0f;

    Target self;
    NavMeshAgent agent;
    Animator animator;
    List<GameObject> enemies;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        self = GetComponent<Target>();

        enemies = new System.Collections.Generic.List<GameObject>();
        populateEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        if (self.health > 0f)
        {
            GameObject currentEnemy;
            if (enemies.Count == 0)
            {
                populateEnemies();
            }
            if (enemies.Count > 0)
            {
                currentEnemy = enemies[0];
                if (currentEnemy != null && enemies.Count > 0)
                {
                    float distanceToEnemy = Vector3.Distance(currentEnemy.transform.position, transform.position);
                    if (distanceToEnemy <= lookRadius)
                    {
                        agent.SetDestination(currentEnemy.transform.position);

                        if (distanceToEnemy <= agent.stoppingDistance)
                        {
                            // Face target
                            FaceTarget(currentEnemy);
                            AttackTarget(currentEnemy);
                        }
                        else
                        {
                            animator.SetBool("Attack", false);
                        }
                    }
                    else
                    {
                        populateEnemies();
                    }
                }
            }
        }
        attackCooldown -= Time.deltaTime;
    }

    void FaceTarget(GameObject enemy)
    {
        Vector3 direction = (enemy.transform.position - transform.position).normalized;
        Quaternion lookRoation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRoation, Time.deltaTime * 5f);
    }

    void AttackTarget(GameObject currentEnemy)
    {
        if (currentEnemy.name.CompareTo("Player") == 0)
        {
            PlayerStats enemy = currentEnemy.transform.GetComponent<PlayerStats>();
            if (attackCooldown <= 0f)
            {
                if (enemy.playerHealth >= 0)
                {
                    animator.SetBool("Attack", true);
                    enemy.TakeDamage(attackDamage);
                    attackCooldown = 2f;
                }
                else
                {
                    animator.SetBool("Attack", false);
                }
            }
        }
        else
        {
            Target enemy = currentEnemy.transform.GetComponent<Target>();
            if (attackCooldown <= 0f)
            {
                if (enemy.health >= 0)
                {
                    animator.SetBool("Attack", true);
                    enemy.TakeDamage(attackDamage);
                    attackCooldown = 2f;
                }
                else
                {
                    animator.SetBool("Attack", false);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    // This function gets an array of all the enemies in range
    void populateEnemies()
    {
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
            foreach (GameObject obj in tagged)
            {
                distToEnemy = Vector3.Distance(obj.transform.position, transform.position);
                if (distToEnemy <= lookRadius)
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
        float distToP1= Vector3.Distance(p1.transform.position, transform.position);
        float distToP2 = Vector3.Distance(p2.transform.position, transform.position);
        Debug.Log(distToP1.CompareTo(distToP2));
        return distToP1.CompareTo(distToP2);
    }
}
