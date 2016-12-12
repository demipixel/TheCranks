using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float moveSpeed = 5;

	Drone drone;
	LivingEntity entity;
	Transform weapon;
	Weapon weaponObj;
	Transform weaponGraphics;
	PlayerController playerController;

	Camera cam;
	public GameController controller;

	// Use this for initialization
	void Start () {
		Transform droneObject = transform.GetChild (0);
		drone = droneObject.GetComponent<Drone> ();
		entity = droneObject.GetComponent<LivingEntity> ();
		weapon = droneObject.Find ("Weapon");
		weaponObj = weapon.GetComponent<Weapon> ();
		weaponGraphics = weapon.GetChild(0);

		weaponObj.weaponCooldown = 0.5f;

		playerController = GetComponent<PlayerController> ();
		playerController.myRigidbody = drone.GetComponent<Rigidbody2D> ();

		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		if (entity.health <= 0) { // Dead
			if (controller.State != GameController.state.dead)
				controller.SetState (GameController.state.dead);
			return;
		} else if (controller.State != GameController.state.off) // Don't allow looking around, moving, or shooting
			return;
		
		Vector2 moveInput = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		Vector2 moveVelocity = moveInput.normalized * moveSpeed;
		playerController.Move (moveVelocity);

		Vector3 mousePos = cam.ScreenToWorldPoint (Input.mousePosition);
		weaponGraphics.rotation = Quaternion.Euler (new Vector3(0, 0, Mathf.Atan2(mousePos.y - weaponGraphics.position.y, mousePos.x - weaponGraphics.position.x)/Mathf.PI*180f + 90f));

		if (Input.GetMouseButton (0)) {
			weaponObj.Shoot ();
		}
	}

	public void Upgrade(int lvl) {
		weaponObj.weaponCooldown = 0.5f * Mathf.Pow (0.85f, lvl - 1);
		weaponObj.damage = 5.0f * Mathf.Pow(1.2f, lvl - 1);
		weaponObj.projectiles = Mathf.Min((lvl + 1) / 2, 3);
	}
}
