using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour {

	public GameController controller;
	Transform playerDrone;

	Rigidbody2D myRigidbody;

	GameObject collectParticle;

	public AudioClip ding;

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> ();
		myRigidbody.AddForce (new Vector2 (Random.Range (-300f, 100f), 250f));

		collectParticle = Resources.Load<GameObject> ("CollectGold");
	}
	
	// Update is called once per frame
	void Update () {
		if (controller.State == GameController.state.dead)
			return;
		if (!playerDrone && controller.PlayerObject != null)
			playerDrone = controller.PlayerObject.transform.Find ("Drone").transform;
		if (!playerDrone) // Dead
			return;
		float sqrDist = Vector3.SqrMagnitude (playerDrone.position - transform.position);
		if (sqrDist < 0.6f*0.6f) {
			Collect ();
		} else if (sqrDist < 2.5f * 2.5f) {
			myRigidbody.AddForce ((new Vector2 (playerDrone.position.x - transform.position.x, playerDrone.position.y - transform.position.y)) * 10);
		}
	}

	void Collect() {
		controller.gold++;
		controller.score++;
		AudioSource.PlayClipAtPoint (ding, Camera.main.transform.position, 0.5f);
		GameObject particle = Instantiate (collectParticle, transform.position, Quaternion.identity);
		Destroy (particle, 2);
		Destroy (gameObject);
	}
}
