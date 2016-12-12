using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public GameObject drone;
	public Sprite droneSprite;

	public GameObject bomb;

	public GameController controller;

	Transform CostDisplay;
	float[] costSectionX = new float[2];
	float[] costSectionY = new float[2];
	int[,] droneCost = {
		{ 35, 25, 15 },
		{ 45, 30, 25 },
		{ 35, 25, 20 }
	};
	int[,] bombCost = {
		{ 45, 35, 30 },
		{ 45, 35, 35 },
		{ 45, 35, 35 }
	};

	Text GoldText;
	int displayedGold = 0;
	Text ScoreText;
	int displayedScore = 0;

	Transform Tutorial;
	Transform UpgradeScreen;
	Transform GameOver;

	Transform tempDrone;
	Transform tempBomb;

	public AudioClip placeSound;
	public AudioClip bombSound;

	// Use this for initialization
	void Start () {
		CostDisplay = transform.Find ("Costs");
		costSectionX [0] = CostDisplay.GetChild (0).transform.transform.position.x + CostDisplay.GetChild (0).transform.localScale.x / 2;
		costSectionX [1] = CostDisplay.GetChild (3).transform.transform.position.x + CostDisplay.GetChild (4).transform.localScale.x / 2;

		costSectionY [0] = CostDisplay.GetChild (2).transform.transform.position.y + CostDisplay.GetChild (2).transform.localScale.y / 2;
		costSectionY [1] = CostDisplay.GetChild (1).transform.transform.position.y + CostDisplay.GetChild (1).transform.localScale.y / 2;

		GoldText = transform.Find ("Canvas").Find ("Gold").GetComponent<Text> ();
		ScoreText = transform.Find ("Canvas").Find ("Score").GetComponent<Text> ();

		Tutorial = transform.Find ("Tutorial");
		UpgradeScreen = transform.Find ("UpgradeScreen");
		GameOver = transform.Find ("GameOver");
	}
	
	// Update is called once per frame
	void Update () {
		if (controller.State == GameController.state.placeDrone) {
			if (!tempDrone) {
				controller.SetState (GameController.state.off);
				return;
			}
			Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			tempDrone.position = new Vector3 (mousePos.x, mousePos.y, 0);

			int cost = GetAreaCost (mousePos, true);

			tempDrone.Find ("CostText").GetChild (0).GetComponent<RectTransform> ().position =
				Camera.main.WorldToScreenPoint (new Vector3 (tempDrone.position.x, tempDrone.position.y + 1f));
			Text t = tempDrone.Find ("CostText").GetChild (0).GetComponent<Text> ();
			t.text = "Cost: " + GetAreaCost (mousePos, true);
			t.color = cost <= controller.gold ? new Color(0.1f, 0.1f, 0.1f) : new Color(1.0f, 0.2f, 0.2f);

			Drone d = tempDrone.GetComponent<Drone> ();
			bool valid = d.validPosition ();
			tempDrone.GetComponent<SpriteRenderer> ().color = valid ?  Color.white : new Color (1f, 0.5f, 0.5f);

			if (Input.GetMouseButtonDown (0)) {
				if (controller.gold >= cost && valid) {
					controller.gold -= cost;
					CreateDrone (mousePos);
				}
			}
		} else if (controller.State == GameController.state.placeBomb) {
			if (!tempBomb) {
				controller.SetState (GameController.state.off);
				return;
			}
			Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			tempBomb.position = new Vector3 (mousePos.x, mousePos.y, 0);

			int cost = GetAreaCost (mousePos, false);

			tempBomb.Find ("CostText").GetChild (0).GetComponent<RectTransform> ().position =
				Camera.main.WorldToScreenPoint (new Vector3 (tempBomb.position.x, tempBomb.position.y + 0.6f));
			Text t = tempBomb.Find ("CostText").GetChild (0).GetComponent<Text> ();
			t.text = "Cost: " + GetAreaCost (mousePos, false);
			t.color = cost <= controller.gold ? new Color(0.1f, 0.1f, 0.1f) : new Color(1.0f, 0.2f, 0.2f);

			if (Input.GetMouseButtonDown (0)) {
				if (controller.gold >= cost) {
					controller.gold -= cost;
					CreateBomb (mousePos);
				}
			}
		} else if (controller.State == GameController.state.off) {
			if (controller.score >= 15 && controller.score < 30) {
				Tutorial.Find ("DroneText").gameObject.SetActive (true);
			} else if (controller.score >= 30 && controller.score < 45) {
				Tutorial.Find ("DroneText").gameObject.SetActive (false);
				Tutorial.Find ("BombText").gameObject.SetActive (true);
			} else if (controller.score >= 45 && controller.score < 60) {
				Tutorial.Find ("BombText").gameObject.SetActive (false);
				Tutorial.Find ("UpgradeText").gameObject.SetActive (true);
			} else if (controller.score >= 60 && controller.score < 65) {
				Tutorial.Find ("UpgradeText").gameObject.SetActive (false);
			}
		}

		if (displayedGold != controller.gold) {
			GoldText.text = controller.gold.ToString();
			displayedGold = controller.gold;
		}
		if (displayedScore != controller.score) {
			ScoreText.text = "Score: "+controller.score;
			displayedScore = controller.score;
		}
	}

	public void CreateTempDrone() {
		tempDrone = Instantiate (drone).transform;
		SpriteRenderer ren = tempDrone.GetComponent<SpriteRenderer> ();
		ren.sprite = droneSprite;
		tempDrone.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Kinematic;
		ren.color = new Color (1, 1, 1, 0.3f);
		ren.sortingLayerName = "UI";
		tempDrone.Find ("CostText").gameObject.SetActive (true);
		tempDrone.parent = transform;
		tempDrone.GetComponent<Drone> ().controller = controller;

		CostDisplay.gameObject.SetActive (true);
		Time.timeScale = 0;
	}

	void CreateDrone(Vector3 pos) {
		DestroyTempDrone ();
		Transform d = Instantiate (drone, new Vector3(pos.x, pos.y), Quaternion.identity).transform;
		d.GetComponent<Rigidbody2D> ().freezeRotation = true;

		SpriteRenderer ren = d.GetComponent<SpriteRenderer> ();
		ren.sprite = droneSprite;

		Drone dr = d.GetComponent<Drone> ();
		dr.playerControlled = false;
		dr.controller = controller;
		// dr.Upgrade (controller.droneLevel);

		controller.drones.Add (d.gameObject);

		d.parent = controller.transform;

		// AudioSource.PlayClipAtPoint (placeSound, Camera.main.transform.position);
		GetComponent<AudioSource>().PlayOneShot(placeSound);
	}

	public void DestroyTempDrone() {
		Destroy (tempDrone.gameObject);
		CostDisplay.gameObject.SetActive (false);
	}

	public void CreateTempBomb() {
		tempBomb = Instantiate(bomb).transform;
		SpriteRenderer ren = tempBomb.GetComponent<SpriteRenderer> ();
		ren.color = new Color (1, 1, 1, 0.3f);
		tempBomb.parent = transform;

		CostDisplay.gameObject.SetActive (true);
		Time.timeScale = 0;
	}

	void CreateBomb(Vector3 pos) {
		DestroyTempBomb ();
		for (int i = controller.enemies.Count-1; i >= 0; i--) {
			Transform t = controller.enemies [i].transform;
			if (Mathf.Abs(t.position.x - pos.x) <= 1.5f && Mathf.Abs(t.position.y - pos.y) <= 1.5f) {
				controller.enemies [i].GetComponent<Enemy> ().TakeDamage (35f, Mathf.Atan2 (t.position.y - pos.y, t.position.x - pos.x));
			}
		}

		//AudioSource.PlayClipAtPoint (bombSound, Camera.main.transform.position, 0.7f);
		GetComponent<AudioSource>().PlayOneShot(bombSound);
	}

	public void DestroyTempBomb() {
		Destroy (tempBomb.gameObject);
		CostDisplay.gameObject.SetActive (false);
	}

	int GetAreaCost(Vector3 pos, bool drone) {
		int costX = 2;
		int costY = 2;
		for (int i = 0; i < 2; i++) {
			if (pos.x < costSectionX [i]) {
				costX = i;
				break;
			}
		}
		for (int i = 0; i < 2; i++) {
			if (pos.y < costSectionY [i]) {
				costY = i;
				break;
			}
		}
		return drone ? droneCost [costY, costX] : bombCost [costY, costX];
	}

	public void ToggleUpgradeScreen(bool show) {
		UpgradeScreen.gameObject.SetActive (show);
	}

	public void RefreshUpgradeScreen() {
		Text PlayerText = UpgradeScreen.Find ("UpgradePlayer").Find ("Text").GetComponent<Text> ();
		Text DroneText = UpgradeScreen.Find ("UpgradeDrones").Find ("Text").GetComponent<Text> ();

		PlayerText.text = "Upgrade Player (Lvl " + controller.playerLevel + ")      " + controller.getPlayerLevelCost (controller.playerLevel);
		DroneText.text = "Upgrade Drones (Lvl " + controller.droneLevel + ")      " + controller.getDroneLevelCost (controller.droneLevel);
	}

	public void ShowGameOver() {
		GameOver.Find ("Score").GetComponent<Text> ().text = "Score: " + controller.score;
		GameOver.Find ("HighScore").GetComponent<Text> ().text = "High Score: " + PlayerPrefs.GetInt ("highscore");
		GameOver.gameObject.SetActive (true);
	}
}
