
using UnityEngine;

public class Target : MonoBehaviour {

	public float health = 50f;
	Animator animator;
	bool dead = false;
	void Start(){
		
		animator = GetComponent<Animator> ();
		animator.SetBool ("dead", dead);

	}

	public void TakeDamage(float amount){
		
		health -= amount;
		Debug.Log (health);
		if (health <= 0f) {

			Die ();
		}
	}

	void Die(){
		dead = true;
		animator.SetBool ("dead", dead);
		//animator.enabled = true;
		Destroy (gameObject, 5f);
	}
}
