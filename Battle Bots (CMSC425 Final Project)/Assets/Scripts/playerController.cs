using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {


	private Rigidbody rb;
	private float speed;
	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		speed = 10;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			//Fire ();
		}
	}

	void FixedUpdate () {

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");




		Vector3 movement = new Vector3 (moveHorizontal, 0, moveVertical);
		Vector3 rotation = new Vector3 (0, moveHorizontal, 0);
		Quaternion deltaRotation = Quaternion.Euler(rotation * speed);
		Vector3 orient = new Vector3 (0, 0, 0);







		if (Input.GetKey (KeyCode.UpArrow)) {

			transform.position += transform.forward * speed * Time.deltaTime;
			if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) {
				rb.MoveRotation (rb.rotation * deltaRotation);

			}

		} else if (Input.GetKey (KeyCode.DownArrow)) {
			transform.position -= transform.forward * speed * Time.deltaTime;
			if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.LeftArrow)) {
				rb.MoveRotation (rb.rotation * deltaRotation);
			}
		} else if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow)) {

			rb.MoveRotation (rb.rotation * deltaRotation);

		} 
		if (Input.GetKeyDown ("space")){


			rb.AddForce(Vector3.up*5, ForceMode.Impulse);

		} 

	}

	void Fire(){

		GameObject bullet = Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10;

		Destroy(bullet, 2.0f);



	}
}
