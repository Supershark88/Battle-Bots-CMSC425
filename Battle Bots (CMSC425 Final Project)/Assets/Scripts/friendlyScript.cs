using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class friendlyScript : MonoBehaviour {
	public float lookRadius = 20f;
	public float attackSpeed = 2f;
	public float range = 50f;
	public float damage = .25f;
	public float fireRate = 1f;
	private float nextTimeToFire = 0f;
	public GameObject impactEffect;

	public GameObject targ;

	private float attackCooldown = 0f;
	public float hitSuccessRate = .25f;

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

	}
	
	// Update is called once per frame
	void Update () {
		if (self.health > 0f)
		{
			float distance = Vector3.Distance(target.position, transform.position);
			int curr = 0;
			GameObject currentEnemy;
			Debug.Log ("Count: "+enemies.Count);
			if (enemies.Count == 0) {
				populateEnemies ();
			} 
			if (enemies.Count > 0) {
				currentEnemy = enemies [curr];
				if (currentEnemy != null && enemies.Count > 0) {
					
					float distanceToEnemy = Vector3.Distance (currentEnemy.transform.position, transform.position);
					if (distanceToEnemy <= lookRadius / 2) {
						agent.updatePosition = false;

						FaceTarget (currentEnemy);

						Target target = currentEnemy.transform.GetComponent<Target> ();
						if (target.health <= 0) {
							enemies.RemoveAt (0);
							animator.SetBool ("Attack", false);
						} else {
							Shoot (currentEnemy, target);
						}

					}
				}
			}

			else if (distance <= lookRadius)
			{
				agent.SetDestination(target.position);


			}
		}
		attackCooldown -= Time.deltaTime;
	}

	void FaceTarget(GameObject enemy)
	{
		Vector3 direction = ( enemy.transform.position - transform.position).normalized;
		Quaternion lookRoation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRoation, Time.deltaTime * 5f);
	}

	void AttackTarget(GameObject enemy, Target target)
	{
		if (attackCooldown <= 0f && target.health > 0) {
			animator.SetBool ("Attack", true);
			target.TakeDamage (damage);

			attackCooldown = 2f;
		} else {
			enemies.Remove (enemy);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, lookRadius);
	}

	void Shoot(GameObject enemy, Target target){
		float success = Random.Range (0, 10f);
		Rigidbody rb = enemy.GetComponent<Rigidbody> ();



		if (target != null) {
			
			if (success >= 8.0f) {
				
				AttackTarget (enemy, target);
				Debug.Log ("Health: " + target.health);

				if (rb != null) {

					rb.AddForce (enemy.transform.position * 30);

				}
				if (impactEffect != null) {
					GameObject impact = Instantiate (impactEffect, new Vector3(enemy.transform.position.x + Random.Range(.5f, 1f), enemy.transform.position.y + Random.Range(0, 2.5f), enemy.transform.position.z) , Quaternion.LookRotation (enemy.transform.position.normalized));
					Destroy (impact, .1f);
				}


			}
		}




		
	}

	void populateEnemies(){
		GameObject[] tagged = GameObject.FindGameObjectsWithTag ("Enemy");
		Debug.Log (tagged.Length);
		if (tagged!=null) {
			
			//enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

			float distToEnemy;
			foreach (GameObject obj in tagged) {
				distToEnemy = Vector3.Distance (obj.transform.position, transform.position);
				if (distToEnemy <= lookRadius) {
					enemies.Add (obj);
					//enemies.Remove (obj);


				}
			}
		}

	}

}
