using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : LivingEntity {

	public bool playerControlled = true;

	Transform weapon;
	Weapon weaponObj;
	Transform weaponGraphics;

	Vector3 startPos;
	float startTime;

	float weaponVelocity;
	float weaponTurnTime = 0.5f;
	GameObject target;

	public GameController controller;

	float HALF_SIZE = 0.75f;

	public override void Start () {
		base.Start ();

		weapon = transform.Find ("Weapon");
		weaponObj = weapon.GetComponent<Weapon> ();
		weaponGraphics = weapon.GetChild(0);

		startPos = transform.position;
		startTime = Time.time;

		if (!playerControlled) {
			Upgrade (controller.droneLevel);
			OnDeath += Died;
		}
	}

	public override void Update () {
		base.Update ();
		if (!playerControlled && controller) {

			if (startPos.y - HALF_SIZE < controller.Background.transform.position.y - controller.Background.transform.localScale.y / 2) {
				startPos.y = controller.Background.transform.position.y - controller.Background.transform.localScale.y / 2 + HALF_SIZE;
			} else if (startPos.y + HALF_SIZE > controller.Background.transform.position.y + controller.Background.transform.localScale.y / 2) {
				startPos.y = controller.Background.transform.position.y + controller.Background.transform.localScale.y / 2 - HALF_SIZE;
			}

			float updownSpeed = 1.0f;
			float amplitude = 0.25f;
			transform.position = new Vector3 (startPos.x, startPos.y + Mathf.Sin ((Time.time - startTime)*updownSpeed)*amplitude, startPos.z);

			if (!target && controller.enemies.Count > 0)
				target = controller.enemies [Random.Range (0, controller.enemies.Count)];

			if (target) {
				float targetRot = (Mathf.Atan2 (target.transform.position.y - weaponGraphics.position.y, target.transform.position.x - weaponGraphics.position.x) / Mathf.PI * 180 + 90 + 360) % 360;
				float rot = Mathf.SmoothDampAngle (weaponGraphics.transform.rotation.eulerAngles.z, targetRot, ref weaponVelocity, weaponTurnTime);
				weaponGraphics.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, rot));

				if (Mathf.Abs (weaponGraphics.transform.rotation.eulerAngles.z - targetRot) < 10) {
					weaponObj.Shoot ();
				}
			}
		}
	}

	public bool validPosition() {
		Vector3 center = transform.position;
		Vector2 halfSize = GetComponent<BoxCollider2D> ().size / 2f;

		Transform back = controller.Background.transform;

		bool backgroundCheck =
			center.x - halfSize.x >= back.position.x - back.localScale.x / 2 &&
			center.x + halfSize.x <= back.position.x + back.localScale.x / 2 &&
			center.y - halfSize.x >= back.position.y - back.localScale.y / 2 &&
			center.y + halfSize.x <= back.position.y + back.localScale.y / 2;

		bool otherDrone = false;
		for (int i = 0; i < controller.drones.Count; i++) {
			otherDrone = GetComponent<SpriteRenderer> ().bounds.Intersects (controller.drones [i].GetComponent<SpriteRenderer> ().bounds);
			if (otherDrone)
				break;
		}
		bool withEnemy = false;
		for (int i = 0; i < controller.enemies.Count; i++) {
			withEnemy = GetComponent<SpriteRenderer> ().bounds.Intersects (controller.enemies [i].GetComponent<SpriteRenderer> ().bounds);
			if (withEnemy)
				break;
		}

		return backgroundCheck && !otherDrone && !withEnemy;
	}

	public void Upgrade(int lvl) {
		weaponObj.weaponCooldown = 0.5f * Mathf.Pow (0.8f, lvl - 1);
		weaponObj.damage = 5.0f * Mathf.Pow(1.2f, lvl - 1);
	}

	void Died() {
		controller.drones.Remove (gameObject);
	}
}
