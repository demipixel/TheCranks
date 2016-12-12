using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crank : MonoBehaviour {

	public GameObject background;
	public GameObject wall;
	public bool up = true;

	float height = 4;
	float crankSpeed = 0;
	bool reverseCranking = false;
	Transform turner;

	bool initialDown = true;

	// Use this for initialization
	void Start () {
		turner = transform.FindChild ("Turner");
	}
	
	// Update is called once per frame
	void Update () {
		float speed = crankSpeed;
		if (speed == 0 && height > 0) // Reverse slowly by default
			speed = -0.5f;
		if (reverseCranking && speed < 0)
			speed = -2f;
		if (initialDown)
			speed = -15f;
		
		float angle = turner.rotation.eulerAngles.z;
		angle += speed * Time.deltaTime * 20;
		turner.rotation = Quaternion.Euler(new Vector3 (0, 0, angle));

		float changeHeight = speed*Time.deltaTime / 20f;
		height += changeHeight;

		int sign = up ? 1 : -1;

		if (height < 0) {
			height = 0;
			initialDown = false;
		} else {
			wall.transform.position = new Vector3 (wall.transform.position.x, wall.transform.position.y + sign * changeHeight, wall.transform.position.z);
			background.transform.localScale -= new Vector3 (0, changeHeight, 0);
			background.transform.position += new Vector3 (0, sign * changeHeight / 2f, 0);
		}
	}

	void OnCollisionEnter2D(Collision2D c) {
		if (c.gameObject.tag == "Enemy") {
			crankSpeed += c.gameObject.GetComponent<Enemy> ().crankSpeed;
		} else if (c.gameObject.tag == "Player") {
			reverseCranking = true;
		}
	}

	void OnCollisionExit2D(Collision2D c) {
		StopTouching (c.gameObject);
	}

	public void StopTouching(GameObject obj) {
		if (obj.tag == "Enemy") {
			crankSpeed -= obj.GetComponent<Enemy> ().crankSpeed;
		} else if (obj.tag == "Player") {
			reverseCranking = false;
		}
	}
}
