using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FireLayer : MonoBehaviour {

	private Tilemap tilemap;
	public GameObject fireReplacement;
	//TODO
	/*
	 * Damage over time when stepped on tile. Right now the level reset after collision.
	*/

	// Use this for initialization
	void Start(){
		tilemap = GetComponent<Tilemap>();
	}


	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "WaterElement"){
			destroyBlock(col);
		}
	}

	void destroyBlock(Collision2D col){
		Debug.Log ("Fire layer collision" + " " + col.contacts.Length);
		Vector3 hitPosition = Vector3.zero;
		if (tilemap != null && col.gameObject.name != "Player"){
			foreach (ContactPoint2D hit in col.contacts){
				hitPosition.x = hit.point.x;
				hitPosition.y = hit.point.y;
				tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
			}
		}
	}
}

