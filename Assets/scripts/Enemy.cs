using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingEntity {

	public float jumpLoop = 1.0f;
	float currentJump = 0.0f;
	public Vector2 jumpForce = new Vector2 (-50, 50);
	GameObject touchingCrank = null;
	List<LivingEntity> touchingDamagable = new List<LivingEntity> ();

	public float damage = 5.0f;
	public float damageFrequency = 1.5f;
	float damageTimer = 1.5f;

	public float crankSpeed = 1.0f;

	public GameController controller;
	Rigidbody2D myRigidbody;

	public int dropGold = 0;
	public GameObject Gold;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		myRigidbody = GetComponent<Rigidbody2D> ();
		OnDeath += Died;
		currentJump = Random.Range (0, jumpLoop);
	}

	public override void Update() {
		base.Update ();
		damageTimer += Time.deltaTime;
		if (damageTimer >= damageFrequency) {
			for (int i = touchingDamagable.Count - 1; i >= 0; i--) {
				if (touchingDamagable [i].health <= 0) {
					touchingDamagable.RemoveAt (i);
				}
			}
			foreach (LivingEntity damagable in touchingDamagable) {
				if (damagable.health <= 0) {
					touchingDamagable.Remove (damagable);
					continue;
				}
				Transform t = damagable.transform;
				damagable.TakeDamage (damage, Mathf.Atan2(t.position.y - transform.position.y, t.position.x - transform.position.x));
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		currentJump += Time.fixedDeltaTime;
		if (currentJump > jumpLoop) {
			currentJump -= jumpLoop;
			myRigidbody.AddForce (jumpForce);
		}
	}

	void OnCollisionEnter2D(Collision2D c) {
		if (c.gameObject.tag == "Crank")
			touchingCrank = c.gameObject;
		else if (c.gameObject.tag == "Player") {
			LivingEntity entity = c.gameObject.GetComponent<LivingEntity> ();
			if (!touchingDamagable.Contains (entity)) {
				touchingDamagable.Add (entity);
				if (touchingDamagable.Count == 1) // Instant damage
					damageTimer = damageFrequency;
			}
		}
	}

	void OnCollisionExit2D(Collision2D c) {
		if (c.gameObject.tag == "Crank")
			touchingCrank = null;
		else if (c.gameObject.tag == "Player") {
			LivingEntity entity = c.gameObject.GetComponent<LivingEntity> ();
			if (touchingDamagable.Contains (entity)) {
				touchingDamagable.Remove (entity);
			}
		}
	}

	void Died() {
		if (touchingCrank)
			touchingCrank.GetComponent<Crank> ().StopTouching (gameObject);
		controller.enemies.Remove (gameObject);
		for (int i = 0; i < dropGold; i++) {
			GameObject gold = Instantiate(Gold, transform.position, Quaternion.identity);
			gold.GetComponent<Gold> ().controller = controller;
		}
	}
}
