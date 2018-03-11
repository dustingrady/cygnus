using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Layer : MonoBehaviour {

	protected Rigidbody2D rb;
	protected Tilemap tilemap;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		tilemap = GetComponent<Tilemap>();
	}

	protected void DestroyBlock(Collider2D col, GameObject anim = null)
	{
		Vector3 hitPosition = Vector3.zero;
		Vector3 blockPosition = Vector3.zero;

		bool tileDestroyed = false;

		Vector3Int cellPos = GetCollidedTile (col.transform.position);

		if (tilemap.GetTile(cellPos) != null)
		{
			blockPosition = tilemap.CellToWorld(cellPos) + tilemap.tileAnchor;
			// Delete tile
			tilemap.SetTile(cellPos, null);
			tileDestroyed = true;
		}

		if (anim != null) {
			GameObject go = Instantiate (anim, blockPosition, Quaternion.identity);	//Replace the tile
			Destroy(go, 2);
		}
	}

	protected Vector3Int GetCollidedTile(Vector3 position) {
		Vector3Int cellPos = tilemap.WorldToCell (position);

		List<Vector3> positionChecks = getPositionChecks (position);

		foreach (Vector3 pos in positionChecks) {
			if (tilemap.GetTile(tilemap.WorldToCell (pos)) != null) {
				cellPos = tilemap.WorldToCell (pos);
			}
		}

		return cellPos;

	}

	protected List<Vector3Int> GetConnectedBlocks(Vector3Int cellPos) {
		List<Vector3Int> validPos = new List<Vector3Int> ();

		for (int i = -1; i < 2; i++) {
			for (int j = -1; j < 2; j++) {
				Vector3Int curPos = new Vector3Int (cellPos.x + i, cellPos.y + j, cellPos.z);
				//Debug.Log (curPos);
				if (tilemap.GetTile(curPos) != null) {
					validPos.Add (curPos);
				}
			}
		}

		return validPos;
	}


	protected void DestroyBlocks(List<Vector3Int> positions, GameObject anim = null)
	{
		Vector3 hitPosition = Vector3.zero;
		Vector3 blockPosition = Vector3.zero;

		bool tileDestroyed = false;

		foreach (Vector3Int cellPos in positions) {
			if (tilemap.GetTile(cellPos) != null)
			{
				blockPosition = tilemap.CellToWorld(cellPos) + tilemap.tileAnchor;
				// Delete tile
				tilemap.SetTile(cellPos, null);
				tileDestroyed = true;
			}

			if (anim != null) {
				GameObject go = Instantiate (anim, blockPosition, Quaternion.identity);	//Replace the tile
				Destroy(go, 2);
			}	
		}
	}


		
	// Collects vectors of all positions slightly above, below, and to the left and right of the given point
	protected List<Vector3> getPositionChecks(Vector3 pos) {
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
