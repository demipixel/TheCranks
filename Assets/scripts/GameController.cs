using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public enum state {start, off, placeDrone, placeBomb, upgrade, dead};
	public state State;

	public GameObject UIObject;
	UIController UI;

	public GameObject PlayerObject;
	Player player;

	public GameObject Background;

	public int gold = 0;
	public int score = 0;
	public List<GameObject> enemies = new List<GameObject>();
	public List<GameObject> drones = new List<GameObject>();

	public int playerLevel = 1;
	public int droneLevel = 1;

	// Use this for initialization
	void Start () {
		UI = UIObject.GetComponent<UIController> ();
		UI.controller = this;

		player = PlayerObject.GetComponent<Player> ();
		player.controller = this;

		SetState (state.start);
	}
	
	// Update is called once per frame
	void Update () {
		/*if (State == state.off)
			SetState (state.placeDrone);*/
		if (State == state.off) {
			if (Background.transform.localScale.y < 0.5) {
				SetState (state.dead);
			} else if (Input.GetKeyDown ("space")) {
				SetState (state.placeDrone);
			} else if (Input.GetKeyDown ("b")) {
				SetState (state.placeBomb);
			} else if (Input.GetKeyDown ("escape")) {
				SetState (state.upgrade);
			}
		} else if (State == state.placeDrone) {
			if (Input.GetKeyDown ("space") || Input.GetKeyDown ("escape")) {
				UI.DestroyTempDrone ();
				SetState (state.off);
			}
		} else if (State == state.placeBomb) {
			if (Input.GetKeyDown ("b") || Input.GetKeyDown ("escape")) {
				UI.DestroyTempBomb ();
				SetState (state.off);
			}
		} else if (State == state.upgrade) {
			if (Input.GetKeyDown ("escape")) {
				SetState (state.off);
			}
		}
	}

	public void SetState(state s) {
		state oldState = State;
		State = s;

		if (State == state.start) {
			Time.timeScale = 0.0f;
		} else if (State == state.off) {
			Time.timeScale = 1.0f;
		} else if (State == state.placeDrone) {
			UI.CreateTempDrone ();
		} else if (State == state.placeBomb) {
			UI.CreateTempBomb ();
		} else if (State == state.upgrade) {
			Time.timeScale = 0.0f;
			UI.ToggleUpgradeScreen (true);
			UI.RefreshUpgradeScreen ();
		} else if (State == state.dead) {
			if (score > PlayerPrefs.GetInt ("highscore")) {
				PlayerPrefs.SetInt ("highscore", score);
			}
			UI.ShowGameOver ();
		}

		if (oldState == state.start && oldState != State) {
			transform.Find ("MainMenu").gameObject.SetActive (false);
		} else if (oldState == state.upgrade) {
			UI.ToggleUpgradeScreen (false);
		}
	}

	public void StartGame() {
		SetState (state.off);
	}

	public void UpgradePlayer() {
		if (getPlayerLevelCost (playerLevel) > gold)
			return;
		gold -= getPlayerLevelCost (playerLevel);

		playerLevel++;
		player.Upgrade (playerLevel);
		UI.RefreshUpgradeScreen ();
	}

	public void UpgradeDrones() {
		if (getDroneLevelCost (droneLevel) > gold)
			return;
		gold -= getDroneLevelCost (droneLevel);

		droneLevel++;
		for (int i = 0; i < drones.Count; i++) {
			drones [i].GetComponent<Drone> ().Upgrade (droneLevel);
		}
		UI.RefreshUpgradeScreen ();
	}

	public int getPlayerLevelCost(int lvl) {
		return 40 + 20 * (lvl - 1);
	}

	public int getDroneLevelCost(int lvl) {
		return 40 + 30 * (lvl - 1);
	}

	public void Restart() {
		Application.LoadLevel (0);
	}
}
