using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public enum Slope : int {
    Positive,
    Negative
}

[CustomGridBrush(true, false, false, "Slope Brush")]
public class SlopeBrush : GridBrush {
    GameObject[] slopes;
    public Slope direction;

    private void OnEnable()
    {
        direction = Slope.Positive;
        slopes = new GameObject[2] {
            Resources.Load<GameObject>("Prefabs/Brushes/Slopes/PosSlopeTile"),
			Resources.Load<GameObject>("Prefabs/Brushes/Slopes/NegSlopeTile")
        };
    }

    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        var paintTile = (Tile)cells[0].tile;
        var paintObject = Instantiate(slopes[(int)direction], new Vector3(position.x + 0.5f, position.y + 0.5f, Vector3.zero.z), Quaternion.identity, brushTarget.transform);
        var sprite = paintObject.GetComponent<SpriteRenderer>();

        paintObject.tag = brushTarget.tag;
        sprite.sprite = paintTile.sprite;
    }
}

[CustomEditor(typeof(SlopeBrush))]
public class SlopeBrushEditor : GridBrushEditor {}