using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour {

    public float lookRadius = 10f;
    public float attackSpeed = 2f;

    public GameObject targ;

    private float attackCooldown = 0f;

    Target self;
    Transform target;
    NavMeshAgent agent;
    Animator animator;
    PlayerStats player;

    // Use this for initialization
    void Start()
    {
        target = targ.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        self = GetComponent<Target>();
        player = targ.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (self.health > 0f)
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position);

                if (distance <= agent.stoppingDistance)
                {
                    // Face target
                    FaceTarget();
                    AttackTarget();
                }
                else
                {
                    animator.SetBool("Attack", false);
                }
            }
        }
        attackCooldown -= Time.deltaTime;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRoation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRoation, Time.deltaTime * 5f);
    }

    void AttackTarget()
    {
        if (attackCooldown <= 0f)
        {
            animator.SetBool("Attack", true);
            player.TakeDamage(5f);
            attackCooldown = 2f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    } 
}
