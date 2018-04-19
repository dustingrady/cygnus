using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR 
using UnityEditor; 


public enum Slope : int {
    Positive,
    Negative,
    PosShallow,
    NegShallow
}

[CustomGridBrush(true, false, false, "Slope Brush")]
public class SlopeBrush : GridBrush {
    GameObject[] slopes;
    public Slope direction;

    private void OnEnable()
    {
        direction = Slope.Positive;
        slopes = new GameObject[4] {
            Resources.Load<GameObject>("Prefabs/Brushes/Slopes/PosSlopeTile"),
			Resources.Load<GameObject>("Prefabs/Brushes/Slopes/NegSlopeTile"),
            Resources.Load<GameObject>("Prefabs/Brushes/Slopes/PosShallowTile"),
            Resources.Load<GameObject>("Prefabs/Brushes/Slopes/NegShallowTile")
        };
    }

    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        if ((int)direction < 2)
        {
            var paintTile = (Tile)cells[0].tile;
            var paintObject = Instantiate(slopes[(int)direction], new Vector3(position.x + 0.5f, position.y + 0.5f, Vector3.zero.z), Quaternion.identity, brushTarget.transform);
            var sprite = paintObject.GetComponent<SpriteRenderer>();

            paintObject.tag = brushTarget.tag;
            sprite.sprite = paintTile.sprite;
        }
        else
        {
            var paintTiles = (from c in cells select (Tile)c.tile).ToArray();
            var paintObject = Instantiate(slopes[(int)direction], new Vector3(position.x + 0.5f, position.y + 0.5f, Vector3.zero.z), Quaternion.identity, brushTarget.transform);
            paintObject.tag = brushTarget.tag;

            for (int i = 0; i < paintObject.transform.childCount; i++)
            {
                var obj = paintObject.transform.GetChild(i).gameObject;
                var sprite = obj.GetComponent<SpriteRenderer>();

                obj.tag = brushTarget.tag;
                sprite.sprite = paintTiles[i].sprite;
            }
        }
    }
}

[CustomEditor(typeof(SlopeBrush))]
public class SlopeBrushEditor : GridBrushEditor {}

#endif