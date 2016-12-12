using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoverboard : MonoBehaviour {

	Transform enemy;
	Vector3 start;

	// Use this for initialization
	void Start () {
		enemy = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (enemy.position.x, transform.position.y, 0);
	}
}
