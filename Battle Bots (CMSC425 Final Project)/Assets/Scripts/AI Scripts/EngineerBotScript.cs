using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EngineerBotScript : MonoBehaviour
{
    public float range = 20f;
    public float attackSpeed = 3f;
    public int attackDamage = 1;

    public float healSpeed;
    public int healAmount;
    public int speed;
    public int level;

    public GameObject player;

    public bool ally;
    private string side;
    private string oppositeside;

    private float attackCooldown = 0f;
    private float healCooldown = 0f;
    private float healDuration = 0f;

    Target self;
    NavMeshAgent agent;
    Animator animator;
    List<GameObject> enemies;
    List<GameObject> allies;

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
            level = PlayerPrefs.GetInt("EngineerLevel");
        }
        else
        {
            side = "Enemy";
            oppositeside = "Friendly";
            level = PlayerPrefs.GetInt("Level");
        }

        self.health = 20 + 5 * level;
        self.maxhealth = self.health;
        healAmount = 5 + 1 * level;
        healSpeed = 8 - .5f * level;
        speed = 4 + 1 * level;
        agent.speed = speed;

        allies = new System.Collections.Generic.List<GameObject>();
        populateAllies();

        enemies = new System.Collections.Generic.List<GameObject>();
        populateEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        if (self.health > 0f)
        {
            Debug.Log(allies.Count);
            if (allies.Count != 0)
            {
                if (healDuration <= 0f)
                {
                    populateAllies();
                    GameObject currentAlly = allies[0];
                    if (currentAlly.transform.GetComponent<Target>().health == currentAlly.transform.GetComponent<Target>().maxHealth())
                    {
                        animator.SetBool("Heal", false);
                        if (ally)
                        {
                            agent.stoppingDistance = 6;
                            agent.SetDestination(player.transform.position);
                        }
                        else
                        {
                            agent.stoppingDistance = 5;
                            agent.SetDestination(currentAlly.transform.position);
                        }
                    }
                    else if (!currentAlly.transform.GetComponent<Target>().isDead())
                    {
                        agent.stoppingDistance = 3;
                        agent.SetDestination(currentAlly.transform.position);
                        float distance = Vector3.Distance(currentAlly.transform.position, transform.position);
                        if (distance <= agent.stoppingDistance && healCooldown < 0)
                        {
                            FaceTarget(currentAlly);
                            HealTarget(currentAlly);
                        }
                        else
                        {
                            animator.SetBool("Heal", false);
                        }
                    }
                }
                else
                {
                    GameObject currentAlly = allies[0];
                    FaceTarget(currentAlly);
                    agent.SetDestination(currentAlly.transform.position);
                }
            }
            else
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
                        if (distanceToEnemy <= range)
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
        }
        attackCooldown -= Time.deltaTime;
        healCooldown -= Time.deltaTime;
        healDuration -= Time.deltaTime;
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
                attackCooldown = attackSpeed;
            }
            else
            {
                enemies.Remove(currentEnemy);
                populateEnemies();
                animator.SetBool("Attack", false);
            }
        }
    }

    void HealTarget(GameObject currentAlly)
    {
        Target ally = currentAlly.transform.GetComponent<Target>();
        if (!ally.isDead())
        {
            Debug.Log("Healing!");
            animator.SetBool("Heal", true);
            ally.Heal(healAmount);
            healCooldown = healSpeed;
            healDuration = 2f;
        }
        else
        {
            animator.SetBool("Heal", false);
            allies.Remove(currentAlly);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    // This function gets an array of all the enemies in range,
    // sorting them by 
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

    // This function gets an array of all the allies in range and sorts them by health
    void populateAllies()
    {
        GameObject[] tagged;
        allies = new System.Collections.Generic.List<GameObject>(); ;
        if (side.CompareTo("Friendly") == 0)
        {
            tagged = GameObject.FindGameObjectsWithTag("Friendly");
        }
        else
        {
            tagged = GameObject.FindGameObjectsWithTag("Enemy");
        }
        if (tagged != null)
        {
            foreach (GameObject obj in tagged)
            {
                if (!obj.transform.GetComponent<Target>().isDead() && (obj.transform.name.CompareTo(gameObject.name) != 0))
                {
                    allies.Add(obj);
                }
                else
                {
                    allies.Remove(obj);
                }
            }
            allies.Sort(SortByHealth);
        }
    }

    int SortByHealth(GameObject p1, GameObject p2)
    {
        float healthP1 = p1.transform.GetComponent<Target>().health;
        float maxhealthP1 = p1.transform.GetComponent<Target>().maxHealth();
        float healthP2 = p2.transform.GetComponent<Target>().health;
        float maxhealthP2 = p2.transform.GetComponent<Target>().maxHealth();
        return (healthP1 / maxhealthP1).CompareTo(healthP2 / maxhealthP2);
    }
}
