using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IceLayer : MonoBehaviour {

	private Tilemap tilemap;
	private GameObject meltedIce;

	// Use this for initialization
	void Start(){
		tilemap = GetComponent<Tilemap>();
		meltedIce = (GameObject)Resources.Load("Prefabs/Particles/MeltedIce");	
	}
		
	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "FireElement"){
			destroyBlock(col);
			GameObject melt = Instantiate (meltedIce, col.transform.position, Quaternion.identity);	//Replace the tile
			Destroy(melt, 2);
		}
	}

	void destroyBlock(Collision2D col){
		Debug.Log ("Ice layer collision" + " " + col.contacts.Length);
		Vector3 hitPosition = Vector3.zero;
		if (tilemap != null && col.gameObject.name != "Player"){
			foreach (ContactPoint2D hit in col.contacts)
			{
				hitPosition.x = hit.point.x;
				hitPosition.y = hit.point.y;
				tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
			}
		}
	}
}
