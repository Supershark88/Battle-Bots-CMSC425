using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {

	Vector2 mouseLook;
	Vector2 smoothV;
	public float sensitivity = 10.0f;
	public float smoothing = 2.0f;

	public GameObject character;

	public float yawSpeed = 0f;
	private float currentYaw = 0f;
	private float currentRoll = 0f;
	// Use this for initialization
	void Start () {

		//character = this.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

		md = Vector2.Scale (md, new Vector2 (sensitivity * smoothing, sensitivity * smoothing));
		smoothV.x = Mathf.Lerp (smoothV.x, md.x, 1f / smoothing);
		smoothV.y = Mathf.Lerp (smoothV.y, md.y, 1f / smoothing);
		mouseLook += smoothV;

		var angleZ = 0;
		


		//transform.rotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
		//transform.rotation = Quaternion.AngleAxis(mouseLook.x, Vector3.up);
		//Debug.Log(mouseLook.y);
		//transform.Rotate(-mouseLook.y * Time.deltaTime, mouseLook.x*Time.deltaTime, 0, Space.World);
		//transform.eulerAngles.z = angleZ;
		transform.eulerAngles += new Vector3(-mouseLook.y*yawSpeed*Time.deltaTime, mouseLook.x*yawSpeed*Time.deltaTime, angleZ);

		
		//character.transform.localRotation = Quaternion.AngleAxis (mouseLook.x, character.transform.up);


		//currentYaw -= Input.GetAxis ("Mouse X") * yawSpeed * Time.deltaTime;
		//currentRoll -= Input.GetAxis("Mouse Y") * yawSpeed * Time.deltaTime;

	}

	void LateUpdate(){

		//transform.RotateAround (transform.position, Vector3.up, currentYaw);
		//transform.RotateAround (transform.position, Vector3.right, currentRoll);

		
	}
}
