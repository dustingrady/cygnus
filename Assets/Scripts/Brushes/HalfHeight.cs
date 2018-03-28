using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

[CustomGridBrush(true, false, false, "Half Height")]
public class HalfHeight : GridBrush
{
    GameObject prefab;

    private void OnEnable()
    {
        prefab = Resources.Load<GameObject>("Prefabs/Brushes/HalfHeight");
    }

    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        var paintTile = (Tile)cells[0].tile;
        var paintObject = Instantiate(prefab, new Vector3(position.x + 0.5f, position.y + 0.5f, Vector3.zero.z), Quaternion.identity, brushTarget.transform);
        var sprite = paintObject.GetComponent<SpriteRenderer>();

        paintObject.tag = brushTarget.tag;
        sprite.sprite = paintTile.sprite;
    }
}

[CustomEditor(typeof(HalfHeight))]
public class HalfHeightEditor : GridBrushEditor { }