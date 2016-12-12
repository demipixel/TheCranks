using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour {

	public GameObject Wall;
	Transform[] walls = new Transform[4];

	void OnValidate () {
		return;
		Transform container = transform.FindChild ("Container");
		if (container != null) {
			Destroy (container);
		}
		container = new GameObject ("Container").transform;
		container.parent = transform;
		for (int i = 0; i < 4; i++) {
			walls [i] = Instantiate (Wall).transform;
			walls [i].parent = container;
		}
		SetWallPositions ();
	}

	void SetWallPositions() {
		Vector3[] posList = {
			new Vector3 (0, 5.1f, 0),
			new Vector3 (0, -5.1f, 0),
			new Vector3 (5.1f, 0, 0),
			new Vector3 (-5.1f, 0, 0)
		};
		for (int i = 0; i < 4; i++) {
			if (i < 2) {
				walls [i].localScale = new Vector3 (10, 0.2f, 0);
			} else {
				walls [i].localScale = new Vector3 (0.2f, 10, 0);
			}
			walls [i].position = posList [i];
		}
	}
}
