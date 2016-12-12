using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public string collide = "Enemy";
	float speed = 10.0f;
	public float damage = 5.0f;

	public AudioClip audio;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float angle = (transform.rotation.eulerAngles.z - 90) / 180f * Mathf.PI;
		transform.position += new Vector3 (Mathf.Cos (angle) * Time.deltaTime * speed, Mathf.Sin (angle) * Time.deltaTime * speed, 0);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == collide) {
			Enemy entity = other.GetComponent<Enemy> ();
			entity.TakeDamage (damage, transform.rotation.eulerAngles.z / 180 * Mathf.PI);
			AudioSource.PlayClipAtPoint (audio, Camera.main.transform.position, 0.3f);
			Destroy (gameObject);
		} else if (other.tag == "Wall") {
			Destroy (gameObject);
		}
	}
}
