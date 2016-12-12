using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour {

	public float health = 100.0f;
	public float maxHealth = 100.0f;
	public Color particleColor = new Color(133/255f, 216/255f, 100/255f);

	public event System.Action OnDeath;
	GameObject deathParticle;

	float invicibleTime = 0.5f;
	float invicibleTimeCount = 0.5f;

	public bool showHealthBar = true;
	Transform healthBar;

	public virtual void Awake() {
		deathParticle = Resources.Load<GameObject> ("DeathParticle");
		maxHealth = health;
	}

	public virtual void Start() {
		if (showHealthBar)
			healthBar = transform.FindChild ("HealthBar").GetChild (0);
	}

	public virtual void Update() {
		invicibleTimeCount += Time.deltaTime;
	}

	public void TakeDamage(float damage, float angle) {
		if (invicibleTimeCount < invicibleTime)
			return;
		health -= damage;
		float forceAmount = 40.0f;
		GameObject particle = Instantiate (deathParticle, transform.position, Quaternion.identity);
		particle.GetComponent<ParticleSystem> ().startColor = particleColor;
		Destroy (particle, 2.0f);
		// particle.GetComponent<ParticleSystem> ().main.startColor = particleColor;
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Mathf.Cos(angle)*forceAmount, Mathf.Sin(angle)*forceAmount));
		if (health <= 0)
			Die ();
		UpdateHealthBar ();
	}

	void UpdateHealthBar() {
		if (!showHealthBar)
			return;
		healthBar.localScale = new Vector3 (health / maxHealth, 1);
		healthBar.localPosition = new Vector3 (-0.5f + health / maxHealth * 0.5f, 0);
		if (health > maxHealth * 0.5)
			healthBar.GetComponent<SpriteRenderer> ().color = Color.Lerp (Color.yellow, new Color (46 / 255f, 208 / 255f, 81 / 255f), ((health / maxHealth) - 0.5f) * 2);
		else
			healthBar.GetComponent<SpriteRenderer> ().color = Color.Lerp (Color.red, Color.yellow, health / maxHealth * 2);
	}

	void Die() {
		if (OnDeath != null)
			OnDeath ();
		Explode ();
		Destroy (gameObject);
	}

	void Explode() { // Explode animation and/or particles

	}
}
