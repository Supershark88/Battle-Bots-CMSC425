
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour {

	public float health = 10f;

	private Animator animator;
    private BoxCollider box;
    private Rigidbody body;
    private NavMeshAgent nav;

	bool dead = false;


	void Start()
    {
        box = GetComponent<BoxCollider> ();
        body = GetComponent<Rigidbody>();
        if (this.tag != "Inanimate")
        {
            animator = GetComponent<Animator>();
            nav = GetComponent<NavMeshAgent>();
        }
	}

	public void TakeDamage(float amount)
    {
		
		health -= amount;

        if (animator != null)
        {

        }

		if (health <= 0f) {

			Die ();
		}
	}

	private void Die(){
		dead = true;
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        if (nav != null)
        {
            nav.speed = 0;
            nav.angularSpeed = 0;
        }
        animator.SetBool ("dead", dead);
        //animator.enabled = true;
        box.enabled = false;
		Destroy (gameObject, 5f);
	}
}
