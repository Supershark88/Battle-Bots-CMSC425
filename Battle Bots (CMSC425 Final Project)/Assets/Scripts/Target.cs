
using UnityEngine;

public class Target : MonoBehaviour {

	public float health = 10f;

	Animator animator;
    BoxCollider box;

	bool dead = false;


	void Start()
    {
		animator = GetComponent<Animator> ();
        box = GetComponent<BoxCollider> ();
	}

	public void TakeDamage(float amount)
    {
		
		health -= amount;
		Debug.Log (health);
		if (health <= 0f) {

			Die ();
		}
	}

	private void Die(){
		dead = true;
		animator.SetBool ("dead", dead);
        //animator.enabled = true;
        box.enabled = false;
		Destroy (gameObject, 5f);
	}
}
