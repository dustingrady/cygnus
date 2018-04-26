using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallingPlatform : MonoBehaviour {

	private Tilemap tilemap;
	public GameObject fallingPrefab;
	List<Tile> prevTile = new List<Tile>();
	List<Vector3> tilesHit;
	TileBase previousTile;
	Vector3 prevBlock;

	// Use this for initialization
	void Start () {
		tilemap = GetComponent<Tilemap>();
	}


	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag == "Player") {
			DestroyBlock (collision);
		}
	}


	void DestroyBlock(Collision2D collision) {
		Vector3 hitPosition = Vector3.zero;
		Vector3 blockPosition = Vector3.zero;

		tilesHit = new List<Vector3>();
		//Tile tileHit;

		bool tileDestroyed = false;

		foreach (ContactPoint2D hit in collision.contacts) {
			hitPosition.x = hit.point.x;
			hitPosition.y = hit.point.y - 0.2f;
			var cellPos = tilemap.WorldToCell (hitPosition);

			if (tilemap.GetTile (cellPos) != null) {
				blockPosition = tilemap.CellToWorld (cellPos) + tilemap.tileAnchor;

				// Get The tile sprite for replacement
				Sprite replacementSprite = tilemap.GetSprite (cellPos);
				fallingPrefab.GetComponent<SpriteRenderer> ().sprite = replacementSprite;

				//prevTile.Add (tilemap.GetTile (cellPos));
				previousTile = tilemap.GetTile (cellPos);
				prevBlock = blockPosition;
				// Delete tile
				tilemap.SetTile (cellPos, null);
				tileDestroyed = true;
				tilesHit.Add (blockPosition);
			}
		}

		if (tileDestroyed) {
			foreach (var location in tilesHit) {
				Instantiate (fallingPrefab, location, Quaternion.identity);
				StartCoroutine (replaceTile (location));
			}
		}
	}

	IEnumerator replaceTile(Vector3 v)
	{
		yield return new WaitForSeconds (5);
		tilemap.SetTile (tilemap.WorldToCell(v), previousTile);
	}

}
