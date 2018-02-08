using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FireLayer : Layer {

	public GameObject fireReplacement;
	//TODO
	/*
	 * Damage over time when stepped on tile. Right now the level reset after collision.
	*/

	// Use this for initialization
	void Start(){
		tilemap = GetComponent<Tilemap>();
	}
		
	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "WaterElement") {
			DestroyBlock(col);
		}
	}

}

