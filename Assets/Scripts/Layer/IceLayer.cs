using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IceLayer : Layer {

	private GameObject meltedIce;

	// Use this for initialization
	void Start(){
		tilemap = GetComponent<Tilemap>();
		meltedIce = (GameObject)Resources.Load("Prefabs/Particles/MeltedIce");	
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "FireElement"){
			DestroyBlock(col, anim : meltedIce);
		}
	}
}
