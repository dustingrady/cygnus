using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour {

    [SerializeField]
    private Stat health;
	public Element leftElement;
	public Element rightElement;
    public Element centerElement;

    Inventory inventory;

    private void Awake()
    {
        health.Initalize();
    }

    void Start () {
		// Inventory references need to be reset each time a scene loads
        inventory = GameObject.Find("Game Manager").GetComponent<Inventory>();
		inventory.initializeInventory ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)) {
            health.CurrentVal -= 10;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            health.CurrentVal += 10;
        }
    }
		
	void OnTriggerEnter2D(Collider2D col) {
		
		// Test for the Playground, if you hit Lava reload
		if (col.gameObject.name == "Lava") {

			inventory.emptyInventory ();
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		}

        if (col.gameObject.tag == "Item") {
			
            //string path = "Items/" + col.gameObject.name;
			//Item temp = Resources.Load(path) as Item;

			Item item = col.gameObject.GetComponent<ItemInteraction>().item;

			if (item != null) {
				inventory.GetComponent<Inventory>().addItem(item);
			} else {
				Debug.LogError ("There was no item on that object!");
			}
        }
    }

	void OnCollisionEnter2D(Collision2D col) {
		if (col.transform.CompareTag("MetalElement") && centerElement.elementType == "magnetic") {

			// Check to see if the metal object hit is a tilemap
			Tilemap tilemap = col.gameObject.GetComponent<Tilemap> ();

			if (tilemap != null) {
				BoxCollider2D bc = GetComponent<BoxCollider2D> ();

				// Get a position to left and right of the player to test for a valid tile
				Vector3 leftBounds = new Vector3 (transform.position.x - bc.bounds.extents.x - 0.1f,
					transform.position.y, 
					transform.position.z);

				Vector3 rightBounds = new Vector3 (transform.position.x + bc.bounds.extents.x + 0.1f,
					transform.position.y, 
					transform.position.z);

				// If a tile is found, the player is hugging the left or right wall, start grapple
				if (tilemap.GetTile (tilemap.WorldToCell (leftBounds)) != null) {
					GetComponent<PlayerController> ().StartGrapple ("right");

					// Disable the magnetic pull
					centerElement.gameObject.GetComponent<Magnetic>().pulling = false;

				} else if (tilemap.GetTile (tilemap.WorldToCell (rightBounds)) != null) {
					GetComponent<PlayerController> ().StartGrapple ("left");

					// Disable the magnetic pull
					centerElement.gameObject.GetComponent<Magnetic>().pulling = false;
				}

			}
		}
	}
}
