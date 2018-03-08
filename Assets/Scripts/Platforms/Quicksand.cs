using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Quicksand : MonoBehaviour
{

    private Tilemap tilemap;
    List<Tile> prevTile = new List<Tile>();
    List<Vector3> tilesHit;
    TileBase previousTile;
    Vector3 prevBlock;

    // Use this for initialization
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

        }

        if (collision.gameObject.tag == "Enemy")
        {

        }
    }
}