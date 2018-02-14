using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SlopeTile : TileBase {
    TileData data = new TileData();
    public Tile orig;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        orig.GetTileData(position, tilemap, ref data);
        /*
        var tileObject = data.gameObject;

        Debug.Log(tileObject.GetComponent<PolygonCollider2D>());

        var collider = tileObject.GetComponent<PolygonCollider2D>();
        collider.CreatePrimitive(3);

        tileData = data;
        */
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        orig.RefreshTile(position, tilemap);
    }
}
