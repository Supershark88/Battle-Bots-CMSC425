using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeBotScript : MonoBehaviour
{

    public float range = 10f;
    public float attackSpeed = 2f;
    public int level;
    public int attackDamage;
    public int speed;


    public GameObject player;

    public bool ally;
    private string side;
    private string oppositeside;

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

        if (ally)
        {
            side = "Friendly";
            oppositeside = "Enemy";
            level = PlayerPrefs.GetInt("BrawlerLevel");
        }
        else
        {
            side = "Enemy";
            oppositeside = "Friendly";
            level = PlayerPrefs.GetInt("Level");
        }

        self.health = 25 + 10 * level;
        self.maxhealth = self.health;
        attackDamage = 3 + 3 * level;
        speed = 2 + 1 * level;
        agent.speed = speed;

        enemies = new System.Collections.Generic.List<GameObject>();
        populateEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        if (self.health > 0f)
        {
            GameObject currentEnemy;
            populateEnemies();
            if (enemies.Count > 0)
            {
                currentEnemy = enemies[0];
                if (currentEnemy != null && enemies.Count > 0)
                {
                    agent.stoppingDistance = 3;
                    float distanceToEnemy = Vector3.Distance(currentEnemy.transform.position, transform.position);
                    if (distanceToEnemy <= range && !currentEnemy.transform.GetComponent<Target>().isDead())
                    {
                        agent.SetDestination(currentEnemy.transform.position);

                        if (distanceToEnemy <= agent.stoppingDistance + .5f)
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
                        enemies.Remove(currentEnemy);
                        populateEnemies();
                    }
                }
            }
            else if (ally)
            {
                agent.stoppingDistance = 4;
                animator.SetBool("Attack", false);
                agent.SetDestination(player.transform.position);
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
        Target enemy = currentEnemy.transform.GetComponent<Target>();
        if (attackCooldown <= 0f)
        {
            if (enemy.isDead() == false)
            {
                animator.SetBool("Attack", true);
                enemy.TakeDamage(attackDamage);
                attackCooldown = 2f;
            }
            else
            {
                enemies.Remove(currentEnemy);
                populateEnemies();
                animator.SetBool("Attack", false);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
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
            RaycastHit hit1;
            RaycastHit hit2;
            foreach (GameObject obj in tagged)
            {
                distToEnemy = Vector3.Distance(obj.transform.position, transform.position);
                if (distToEnemy <= range && obj.transform.GetComponent<Target>().health > 0
                    && ((Physics.Raycast(transform.position + new Vector3(0, 0f, 0), (obj.transform.position - transform.position).normalized, out hit1, 100f)
                    && hit1.transform.tag == oppositeside) ||
                    (Physics.Raycast(transform.position + new Vector3(0, .5f, 0), (obj.transform.position - transform.position).normalized, out hit2, 100f)
                    && hit2.transform.tag == oppositeside)))
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
