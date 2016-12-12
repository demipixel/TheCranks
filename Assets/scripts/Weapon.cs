using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public GameObject Projectile;
	Transform graphics;

	public float weaponCooldown = 1.0f;
	public float damage = 5.0f;
	public int projectiles = 1;
	float weaponTimer = 1.0f;

	// Use this for initialization
	void Start () {
		graphics = transform.GetChild (0);
	}
	
	// Update is called once per frame
	void Update () {
		weaponTimer += Time.deltaTime;
	}

	public void Shoot() {
		if (weaponTimer < weaponCooldown)
			return;

		weaponTimer = 0;
		float angle = graphics.rotation.eulerAngles.z / 180f * Mathf.PI - Mathf.PI/2;
		float offsetAngle = (projectiles - 1) / 5f;
		for (int i = 0; i < projectiles; i++) {
			float a = (angle - offsetAngle) + (2 * offsetAngle / projectiles * i);
			float moveX = Mathf.Cos (a);
			float moveY = Mathf.Sin (a);
			float amt = Projectile.transform.localScale.y * 1.3f;
			GameObject proj = Instantiate (Projectile, graphics.position + new Vector3 (moveX * amt, moveY * amt, 0), Quaternion.Euler(new Vector3(0, 0, a/Mathf.PI*180 + 90)));
			Projectile p = proj.GetComponent<Projectile> ();
			p.damage = damage;
		}
	}
}
