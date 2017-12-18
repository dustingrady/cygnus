using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    [SerializeField]
    private Stat health;
	public Element leftElement;
	public Element rightElement;
    public Element centerElement;
	public Dictionary<string, Element> elements;

    Inventory inventory;


    private void Awake()
    {
        health.Initalize();
    }

    void Start () {
		// Initialize a dictionary of elements that can be found in the world
		elements = new Dictionary<string, Element> ();
		Element fire = GetComponentInChildren<Fire> ();
		elements.Add("fire", fire);

		Element water = GetComponentInChildren<Water> ();
		elements.Add("water", water);

		Element earth = GetComponentInChildren<Earth> ();
		elements.Add ("earth", earth);

		Element metal = GetComponentInChildren<Metal> ();
		elements.Add ("metal", metal);

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

        if (col.gameObject.tag == "Item")
        {
            //Debug.Log("Collided with item");
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
}
