using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MetalLayer : MonoBehaviour {

	private Tilemap tilemap;
	private const int maxHitPoints = 3;
	public int hitpoints = maxHitPoints;
	Vector3 oldPos = Vector3.zero;

	// Use this for initialization
	void Start()
	{
		tilemap = GetComponent<Tilemap>();
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		//THIS IS TEST ELEMENT TAG. IT SHOULD BE WHATEVER LAVA ELEMENT IS WHEN IT'S IMPLEMENTED
		if (col.gameObject.tag == "FireElement")
		{
			DestroyBlock(col);
		}
	}


	void DestroyBlock(Collider2D col)
	{
		Vector3 blockPosition = Vector3.zero;

		Vector3Int cellPos = tilemap.WorldToCell (col.transform.position);

		List<Vector3> positionChecks = getPositionChecks (col.transform.position);

		foreach (Vector3 pos in positionChecks) {
			if (tilemap.GetTile(tilemap.WorldToCell (pos)) != null) {
				cellPos = tilemap.WorldToCell (pos);
			}
		}
			
		//check if player is hitting the same tile. if they move on to a new tile, the old tile will regenerate to full health. Decrement health of tile when player hit same tile 3 times.
		//hacky way of getting around the fact that the health value applied to the whole layer.
		if (Vector3Int.FloorToInt (oldPos) != Vector3Int.FloorToInt (cellPos)) {
			oldPos = cellPos;
			Debug.Log (oldPos.x + " " + oldPos.y + " " + oldPos.z);
			hitpoints = maxHitPoints;
			hitpoints--;
		} else if(Vector3Int.FloorToInt (oldPos) == Vector3Int.FloorToInt (cellPos)){
			if (hitpoints > 0) {
				hitpoints--;
			}
		}

		Debug.Log ("oldPos: " + oldPos + " cellPos: " + cellPos + " hitpoints: " + hitpoints); 

		if (tilemap.GetTile(cellPos) != null && hitpoints == 0)
		{
			hitpoints = maxHitPoints;
			blockPosition = tilemap.CellToWorld(cellPos) + tilemap.tileAnchor;

			// Delete tile
			tilemap.SetTile(cellPos, null);
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
}

