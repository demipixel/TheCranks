using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHelmet : MonoBehaviour {

	public Sprite[] helmets;

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer> ().sprite = helmets [Random.Range (0, helmets.Length)];
	}
}
