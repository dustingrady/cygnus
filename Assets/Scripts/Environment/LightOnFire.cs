/*Function: Instantiate a 'fire layer' over a given area (attach to desired layer)
* Status: Working/ Tested
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LightOnFire : MonoBehaviour {
	private Tilemap tilemap;
	public GameObject lavaPlatform;
	public GameObject[] lavaTiles;
	ParticleSystem ps;
	ParticleSystem.Particle[] plist;

	// Use this for initialization
	void Start () {
		tilemap = GetComponent<Tilemap>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*Light platform on fire by instantiating lava prefab over affected area*/
	public void Catch_Fire(Collider2D col){
		Vector3 blockPosition = Vector3.zero;
		Vector3Int cellPos = tilemap.WorldToCell (col.transform.position);

		List<Vector3> positionChecks = getPositionChecks (col.transform.position);

		foreach (Vector3 pos in positionChecks) {
			if (tilemap.GetTile(tilemap.WorldToCell (pos)) != null) {
				cellPos = tilemap.WorldToCell (pos);
			}
		}

		if (tilemap.GetTile(cellPos) != null){
			blockPosition = tilemap.CellToWorld(cellPos) + tilemap.tileAnchor; //Get The tile sprite for replacement
			Instantiate (lavaPlatform, blockPosition, Quaternion.identity);	//Replace the tile
		}
	}
		

	// Collects vectors of all positions slightly above, below, and to the left and right of the given point
	List<Vector3> getPositionChecks(Vector3 pos) {
		List<Vector3> positionChecks = new List<Vector3> ();
		Vector3 upLeftPos, downLeftPos, leftPos, upRightPos, downRightPos, rightPos;
		upLeftPos = downLeftPos = leftPos = upRightPos = downRightPos = rightPos = pos;

		// Check from the left
		leftPos.x += 0.4f;
		positionChecks.Add (leftPos);

		// Check from the upper left
		upLeftPos.x += 0.4f;
		upLeftPos.y += 0.4f;
		positionChecks.Add (upLeftPos);

		// Check from the lower left
		downLeftPos.x += 0.4f;
		downLeftPos.y -= 0.4f;
		positionChecks.Add (downLeftPos);

		// Check from the right
		rightPos.x -= 0.4f;
		positionChecks.Add (rightPos);

		// Check from the upper right
		upRightPos.x -= 0.4f;
		upRightPos.y += 0.4f;
		positionChecks.Add (upRightPos);

		// Check from the lower right
		downRightPos.x -= 0.4f;
		downRightPos.y -= 0.4f;
		positionChecks.Add (downRightPos);

		return positionChecks;
	}
		
	/*Holds position of all current lava squares, returnes true if passed value is found*/
	private bool Check_For_Lava(Vector3 passedPos){
		lavaTiles = GameObject.FindGameObjectsWithTag ("LavaPlatform");
		foreach(GameObject tile in lavaTiles){
			if(Vector3.SqrMagnitude(tile.transform.position - passedPos) < 1.0){
				return true;
			}
		}
		return false;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "BossBullet") {
			if (!Check_For_Lava (col.transform.position)) {
				Catch_Fire (col); 
			}
		}
	}

	//NEEDS WORK
	/*
	void OnParticleCollision(GameObject other){

		ps = other.GetComponent<ParticleSystem> ();
		plist = new ParticleSystem.Particle[ps.particleCount];

		ps.GetParticles (plist);
		if (other.gameObject.tag == "Lava") {
			
			for (int i = 0; i < plist.Length; i++) {
				Vector3 temp = new Vector3 (plist [i].position.x, plist [i].position.z - 1, 0);
				temp = other.transform.TransformPoint (temp);
				if (!Check_For_Lava (temp)) {
					catch_fire (temp);
					Debug.Log (temp + " " + other.transform.position);
				}
			}
		}
	}


	public void catch_fire(Vector3 position){
		Vector3 blockPosition = Vector3.zero;
		Vector3Int cellPos = tilemap.WorldToCell (position);

		List<Vector3> positionChecks = getPositionChecks (position);

		foreach (Vector3 pos in positionChecks) {
			if (tilemap.GetTile(tilemap.WorldToCell (pos)) != null) {
				cellPos = tilemap.WorldToCell (pos);
			}
		}

		if (tilemap.GetTile(cellPos) != null){
			blockPosition = tilemap.CellToWorld(cellPos) + tilemap.tileAnchor; //Get The tile sprite for replacement
			Instantiate (lavaPlatform, blockPosition, Quaternion.identity);	//Replace the tile
		}
	}*/
}
