using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public int health;
	public Element leftElement;
	public Element rightElement;
	public Dictionary<string, Element> elements;

    Inventory inventory;

	void Start () {
		// Initialize a dictionary of elements that can be found in the world
		elements = new Dictionary<string, Element> ();
		Element fire = GetComponentInChildren<Fire> ();
		elements.Add("fire", fire);

		Element water = GetComponentInChildren<Water> ();
		elements.Add("water", water);

        //Find inventory on game manager
        inventory = GameObject.Find("Game Manager").GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	void Update () {
			
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Item")
        {
            Debug.Log("Collided with item");
            string path = "Prefabs/Items/"+ col.gameObject.name;
            GameObject temp = Resources.Load(path) as GameObject;
            inventory.GetComponent<Inventory>().addItem(temp);
        }
    }
}
