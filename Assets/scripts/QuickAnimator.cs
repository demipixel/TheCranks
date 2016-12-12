using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickAnimator : MonoBehaviour {

	public Sprite[] animationSprites;

	SpriteRenderer spr;
	float frameTimeCounter = 0;
	int currentFrame = 0;
	public float frameTime = 0.05f;
	bool forward = true;

	// Use this for initialization
	void Start () {
		spr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		frameTimeCounter += Time.deltaTime;
		if (frameTimeCounter >= frameTime) {
			frameTimeCounter -= frameTime;
			if (forward) {
				currentFrame++;
				if (currentFrame == animationSprites.Length) {
					currentFrame = animationSprites.Length - 2;
					forward = false;
				}
			} else {
				currentFrame--;
				if (currentFrame == -1) {
					currentFrame = 1;
					forward = true;
				}
			}
			spr.sprite = animationSprites [currentFrame];
		}
	}
}
