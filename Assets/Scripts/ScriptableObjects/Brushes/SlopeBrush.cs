using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

[CustomGridBrush(true, false, false, "Slope Brush")]
public class SlopeBrush : GridBrush {
    /*
    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        base.Paint(gridLayout, brushTarget, position);
        var tilemap = brushTarget.GetComponent<Tilemap>();
        var collider = tilemap.
    }
    */

    public override void Pick(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Vector3Int pickStart)
    {
        base.Pick(gridLayout, brushTarget, position, pickStart);

        foreach (var cell in cells) {
            var cellTile = (Tile)cell.tile;
            //cellTile.colliderType = Tile.ColliderType.Sprite;
        }
    }
}

[CustomEditor(typeof(SlopeBrush))]
public class SlopeBrushEditor : GridBrushEditor { }