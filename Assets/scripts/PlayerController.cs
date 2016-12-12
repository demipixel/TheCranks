using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

	float maxVelocity = 20.0f;
	float axisMaxVelocity;
	Vector2 velocity;
	public Rigidbody2D myRigidbody;

	// Use this for initialization
	void Start () {
		axisMaxVelocity = Mathf.Sqrt (maxVelocity) * Mathf.Sqrt (2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Move(Vector2 vel) {
		velocity = vel;
	}

	void FixedUpdate() {
		// myRigidbody.MovePosition (myRigidbody.position + velocity * Time.fixedDeltaTime);
		if (myRigidbody == null)
			return;
		Vector2 force = velocity * Time.deltaTime * 10;

		float x = Mathf.Sign(myRigidbody.velocity.normalized.x) == Mathf.Sign(force.normalized.x) && Mathf.Abs(myRigidbody.velocity.x) > axisMaxVelocity ? 0 : force.x;
		float y = Mathf.Sign(myRigidbody.velocity.normalized.y) == Mathf.Sign(force.normalized.y) && Mathf.Abs(myRigidbody.velocity.y) > axisMaxVelocity ? 0 : force.y;
		myRigidbody.AddForce (new Vector2(x, y));
		
		myRigidbody.MoveRotation (-myRigidbody.velocity.x*4);
	}
}
