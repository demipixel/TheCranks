using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fizzy : MonoBehaviour {

	public Sprite[] sprites;

	Color[] particleColors = {
		new Color (224/255f, 111/255f, 139/255f),
		new Color (111/255f, 224/255f, 189/255f),
		new Color (111/255f, 193/255f, 224/255f),
		new Color (224/255f, 224/255f, 111/255f)
	};

	// Use this for initialization
	void Start () {
		int spriteNum = Random.Range (0, sprites.Length);
		GetComponent<SpriteRenderer> ().sprite = sprites [spriteNum];
		GetComponent<LivingEntity> ().particleColor = particleColors [spriteNum];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
