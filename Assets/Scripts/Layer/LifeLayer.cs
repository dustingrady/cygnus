using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LifeLayer : Layer {

	private GameObject burningBlock;

	// Use this for initialization
	void Start () {
		tilemap = GetComponent<Tilemap>();
		burningBlock = (GameObject)Resources.Load("Prefabs/Tile Replacements/Burning Block");	
	}

	// When hit by a trigger object (fireball)
	// Instantly destroy the block and replace with the burning block
	void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.tag == "FireElement" && 
			col.gameObject.GetComponent<BurningPlatform>() == null){
			DestroyBlock(col, anim : burningBlock);

			List<Vector3Int> connected = GetConnectedBlocks(GetCollidedTile (col.transform.position));
			StartCoroutine ("spread", connected);


		}
	}
		
	IEnumerator spread(List<Vector3Int> connected) {
		yield return new WaitForSeconds (1);
		DestroyBlocks (connected, anim : burningBlock);

		List<Vector3Int> ajacent = new List<Vector3Int> ();
		foreach (Vector3Int pos in connected) {
			foreach (Vector3Int neighbor in GetConnectedBlocks(GetCollidedTile(pos))) {
				if (!ajacent.Contains (neighbor)) {
					ajacent.Add (neighbor);
				}
			}
		}

		if (ajacent.Count > 0)
			StartCoroutine ("spread", ajacent);
	}

}
